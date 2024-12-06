using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

 
    public class RSAAlgorithm
    {
       
            
            public void MakeKey(string mainPath,string pubKeyPath,string priKeyPath)
            {
                //lets take a new CSP with a new 2048 bit rsa key pair
                RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);

                //how to get the private key
                RSAParameters privKey = csp.ExportParameters(true);

                //and the public key ...
                RSAParameters pubKey = csp.ExportParameters(false);
                //converting the public key into a string representation
                string pubKeyString;
                {
                    //we need some buffer
                    var sw = new StringWriter();
                    //we need a serializer
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                    //serialize the key into the stream
                    xs.Serialize(sw, pubKey);
                    //get the string from the stream
                    pubKeyString = sw.ToString();
                    File.WriteAllText(pubKeyPath, pubKeyString);
                }
                string privKeyString;
                {
                    //we need some buffer
                    var sw = new StringWriter();
                    //we need a serializer
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                    //serialize the key into the stream
                    xs.Serialize(sw, privKey);
                    //get the string from the stream
                    privKeyString = sw.ToString();
                    File.WriteAllText(priKeyPath, privKeyString);
                }
            }
            public string EncryptFile(string inputString,string pubKeyPath)
            {

            //converting the public key into a string representation
            string pubKeyString;
            {
                using (StreamReader reader = new StreamReader(pubKeyPath)) { pubKeyString = reader.ReadToEnd(); }
            }
            //get a stream from the string
            var sr = new StringReader(pubKeyString);

            //we need a deserializer
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));

            //get the object back from the stream
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            csp.ImportParameters((RSAParameters)xs.Deserialize(sr));
            byte[] bytesPlainTextData = Encoding.UTF8.GetBytes(inputString);

            //apply pkcs#1.5 padding and encrypt our data 
            var bytesCipherText = csp.Encrypt(bytesPlainTextData, false);
            //we might want a string representation of our cypher text... base64 will do
            var encryptedText = Convert.ToBase64String(bytesCipherText);
            
            return encryptedText;
        }
            public string DecryptFile(string encryptString,string priKeyPath)
            {
                //we want to decrypt, therefore we need a csp and load our private key
                RSACryptoServiceProvider csp = new RSACryptoServiceProvider();

                string privKeyString;
                {
                    privKeyString = File.ReadAllText(priKeyPath);
                    //get a stream from the string
                    var sr = new StringReader(privKeyString);
                    //we need a deserializer
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                    //get the object back from the stream
                    RSAParameters privKey = (RSAParameters)xs.Deserialize(sr);
                    csp.ImportParameters(privKey);
                }
            var base64Encrypted = encryptString;
            string encryptedText= encryptString;
            //using (StreamReader reader = new StreamReader(filePath)) { encryptedText = reader.ReadToEnd(); }
            
            var resultBytes = Convert.FromBase64String(base64Encrypted);

            //decrypt and strip pkcs#1.5 padding
           // var bytesPlainTextData = csp.Decrypt(bytesCipherText, false);

            var bytesPlainTextData = csp.Decrypt(resultBytes, false);
            var decryptedData = Encoding.UTF8.GetString(bytesPlainTextData);
         
            //get our original plainText back...
            
            return decryptedData;
            }
       

     

//public void GenerateKeyPairAndWriteToFiles(string userName, string userEmail, string publicKeyFilePath, string privateKeyFilePath)
//{
//    // Concatenate the user attributes into a single string
//    string userAttributes = userName + userEmail;

//    // Generate a key pair using the user attributes
//    using (var rsa = new RSACryptoServiceProvider())
//    {
//        var privateKeyParams = rsa.ExportParameters(true);
//        var publicKeyParams = rsa.ExportParameters(false);
//        var keyGenerator = new Rfc2898DeriveBytes(userAttributes, 16);
//        privateKeyParams.Modulus = keyGenerator.GetBytes(128);
//        privateKeyParams.Exponent = new byte[] { 1, 0, 1 };

//        // Export the public and private keys as byte arrays
//        byte[] publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();
//        byte[] privateKeyBytes = rsa.ExportPkcs8PrivateKey();

//        // Write the public and private keys to separate files
//        File.WriteAllBytes(publicKeyFilePath, publicKeyBytes);
//        File.WriteAllBytes(privateKeyFilePath, privateKeyBytes);
//    }
//}

    }

 
    
 