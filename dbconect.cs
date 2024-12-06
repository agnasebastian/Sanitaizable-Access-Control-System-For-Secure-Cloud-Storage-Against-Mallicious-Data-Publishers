using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Text;
using System.Security.Cryptography;


public class dbconect
{
    SqlConnection con = new SqlConnection("server=database-1.ctm4e0wkytbd.us-east-1.rds.amazonaws.com;uid=admin;pwd=admin123;database=SACS_InCloudComputing");
    SqlCommand cmd;
    SqlDataAdapter adp;
    SqlDataReader rd;
    DataSet ds = new DataSet();

    public bool extnon(string str)
    {
        con.Close();
        try
        {
            cmd = new SqlCommand(str, con);
            con.Open();
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }


    }
    public SqlDataReader extread(string str)
    {
        con.Close();
        cmd = new SqlCommand(str, con);
        con.Open();
        rd = cmd.ExecuteReader();
        return rd;

    }
    public string extscalr(string str)
    {
        con.Close();
        cmd = new SqlCommand(str, con);
        con.Open();
        string name = cmd.ExecuteScalar().ToString();
        return name;
    }
    public DataSet discont(string str)
    {
        adp = new SqlDataAdapter(str, con);
        adp.Fill(ds);
        return ds;
    }
    public string MakePwd(int pl)
    {
        string possibles = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        char[] passwords = new char[pl];
        Random rd = new Random();
        for (int i = 0; i < pl; i++)
        {
            passwords[i] = possibles[rd.Next(0, possibles.Length)];

        }
        return new string(passwords);


    }

    public string numpassword(int pl)
    {
        {
            string possibles = "&@$1234567890";
            char[] passwords = new char[pl];
            Random rd = new Random();
            for (int i = 0; i < pl; i++)
            {
                passwords[i] = possibles[rd.Next(0, possibles.Length)];

            }
            return new string(passwords);
        }
    }


    public static int dtkeysize = 1024;
    public string createhash(string data)
    {
        int keysize = dtkeysize / 8;
        byte[] bytes = Encoding.UTF32.GetBytes(data);
        byte[] byt = SHA1.Create().ComputeHash(bytes);
        string Hashvalue = Convert.ToBase64String(byt);
        return Hashvalue;
    }
}