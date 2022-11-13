using System.Data.SqlClient;
using System.Configuration;
using NLog;
using System.Text.RegularExpressions;
using System.IO;

namespace EsercizioRipasso2
{
    public class ClassConn
    {
        bool flag = false;
        SqlConnection dataconn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        public bool isDBOpen;
        bool fla1 = true;
        string Matricola = string.Empty;
        internal static string salvattaggio = File.ReadAllText(ConfigurationManager.AppSettings["Save"].ToString());
        bool Prima_Creazione = Convert.ToBoolean(salvattaggio);


        public void checkConn()
        {
            if (dataconn.State != System.Data.ConnectionState.Open)
            {
                dataconn.Open();
                isDBOpen = true;
            }
        }
        public void closeconn()
        {
            if (dataconn.State == System.Data.ConnectionState.Open)
            {
                dataconn.Close();

            }
        }
        public void connDb()
        {
            try
            {
                string dbcon = ConfigurationManager.AppSettings["KayConnect"].ToString();
                dataconn.ConnectionString = dbcon;
                dataconn.Open();
                isDBOpen = true;

            }
            catch (Exception ex)
            {
                using (SqlCommand comand = cmd)
                {
                    comand.CommandText = ConfigurationManager.AppSettings["Log"].ToString();
                    comand.Parameters.AddWithValue("@Descrizione", ex.Message.ToString());
                    comand.Parameters.AddWithValue("@data", DateTime.Now);
                    comand.Connection = dataconn;
                    comand.ExecuteNonQuery();
                    comand.Parameters.Clear();
                }
                throw;
            }
        }
        public void CreateDataBae()
        {
            try
            {
                checkConn();

                if (isDBOpen && Prima_Creazione == false)
                {
                    using (SqlCommand comand = new SqlCommand())
                    {
                        comand.CommandText = ConfigurationManager.AppSettings["CreateTable"].ToString();
                        comand.Connection = dataconn;
                        comand.ExecuteNonQuery();
                    }

                    using (StreamWriter sw = File.CreateText(ConfigurationManager.AppSettings["Save"].ToString()))
                    {
                        sw.WriteLine("true");
                    }

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                using (SqlCommand comand = cmd)
                {
                    comand.CommandText = ConfigurationManager.AppSettings["Log"].ToString();
                    comand.Parameters.AddWithValue("@Descrizione", ex.Message.ToString());
                    comand.Parameters.AddWithValue("@data", DateTime.Now);
                    comand.Connection = dataconn;
                    comand.ExecuteNonQuery();
                    comand.Parameters.Clear();
                }
                throw;
            }


        }
        public void InsertDataIntheTable()
        {
            checkConn();
            try
            {
                if (isDBOpen)
                {
                    do
                    {
                        Console.WriteLine("Ciao, inserici i dai del Dipendent\n iniza con l'ID ricorda che l'ID deve avere un valore di un numero intero");
                        int id = Convert.ToInt16(Console.ReadLine());
                        Console.WriteLine("Inserisci la via");
                        string via = Console.ReadLine();
                        Console.WriteLine("Inserisci il numero civico");
                        string NumeroCivico = Console.ReadLine();
                        Console.WriteLine("inserisci il Nome della citta");
                        string citta = Console.ReadLine();
                        Console.WriteLine("inserisci il Cap");
                        string cap = Console.ReadLine();
                        Console.WriteLine("inserisci il numero di telefono");
                        string numTel = Console.ReadLine();
                        Console.WriteLine("Inserisci la matricola");

                        do
                        {
                            Matricola = Console.ReadLine();
                            Regex regex = new Regex(@"^\A[A-Z]\w{3}$");
                            if (regex.IsMatch(Matricola))
                            {
                                Console.WriteLine("la Mareicola ha un formato corretto");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("La Matricola non ha un formato corretto, scrivi nuovamente");
                            }

                        } while (true);

                        do
                        {
                            Console.WriteLine("vuoi Confermare i dati? (S/N)");
                            string Risp = Console.ReadLine();
                            if (Risp.ToUpper() == "S")
                            {
                                CreateDataBae();
                                break;
                            }
                            else if (Risp.ToUpper() == "N")
                            {
                                return;
                            }
                            else
                            {
                                Console.WriteLine("La risposa inserita non è coretta");
                            }

                        } while (true);

                        using (SqlCommand comand = cmd)
                        {
                            comand.CommandText = ConfigurationManager.AppSettings["InsertData"].ToString();
                            comand.Parameters.AddWithValue("@id_indirizzo", id);
                            comand.Parameters.AddWithValue("@via", via);
                            comand.Parameters.AddWithValue("@Numero_Civico", NumeroCivico);
                            comand.Parameters.AddWithValue("@citta", citta);
                            comand.Parameters.AddWithValue("@Cap", cap);
                            comand.Parameters.AddWithValue("@Numero_Tel", numTel);
                            comand.Parameters.AddWithValue("@Matricola_Impiegato", Matricola);
                            comand.Connection = dataconn;
                            comand.ExecuteNonQuery();
                            comand.Parameters.Clear();
                        }

                        Console.WriteLine("i dati sono stati inseriti correttamente, ne vuoi inserire altri? (S/N)");
                        do
                        {
                            string risp = Console.ReadLine();

                            if (risp.ToUpper() == "S")
                            {
                                break;
                            }
                            else if (risp.ToUpper() == "N")
                            {
                                fla1 = false;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("non è stata selezionata una opzione Corretta\n");
                                Console.WriteLine("scrivi nuovamente uno dei due comandi (S/N)");
                            }
                        } while (true);

                    } while (fla1);

                }
                else
                {

                    throw new Exception();
                    Exception ex;
                    Console.WriteLine(ex.Message);
                    using (SqlCommand comand = cmd)
                    {
                        comand.CommandText = ConfigurationManager.AppSettings["Log"].ToString();
                        comand.Parameters.AddWithValue("@Descrizione", ex.Message.ToString());
                        comand.Parameters.AddWithValue("@data", DateTime.Now);
                        comand.Connection = dataconn;
                        comand.ExecuteNonQuery();
                        comand.Parameters.Clear();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                using (SqlCommand comand = cmd)
                {
                    comand.CommandText = ConfigurationManager.AppSettings["Log"].ToString();
                    comand.Parameters.AddWithValue("@Descrizione", ex.Message.ToString());
                    comand.Parameters.AddWithValue("@data", DateTime.Now);
                    comand.Connection = dataconn;
                    comand.ExecuteNonQuery();
                    comand.Parameters.Clear();
                }

                throw;
            }
            finally { closeconn(); }
        }
    }

}
