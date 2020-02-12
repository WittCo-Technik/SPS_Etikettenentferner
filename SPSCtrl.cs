using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp7;

namespace SPSCtrl
{
    public class SPSClass
    {
        private static readonly int buffersize = 128; //Buffergröße in Bytes
        private static readonly int byteoffset = 126; //Offset zwischen einzelnen Einträgen (X: 0-61, Y: 62-123, Einträge: 124-126
        private static string IPAdr; //IP-Adresse
        private static int rack, slot;
        static S7Client SPS;
        public static readonly int arraysize = 31;
        private static bool connection;

        private static short Count;
        private static short[] X_Koor = new short[arraysize], Y_Koor = new short[arraysize], w_X_Koor = new short[arraysize], w_Y_Koor = new short[arraysize];
        private static byte[] buffer = new byte[buffersize];
        private static byte[] writebuffer = new byte[buffersize];


        public SPSClass(string IP, int rk = 0, int st = 0) //Konstruktor
        {
            IPAdr = IP;
            rack = rk;
            slot = st;

            SPS = new S7Client(); //Neue Verbindung erzeugen
            SPS.ConnectTo(IPAdr, rk, st); //IP_Adresse, Rack, Slot
            connection = SPS.Connected ? true : false;
        }

        public void ReadDBEntry(int entry)
        {
            if (connection)
            {
                Console.WriteLine("Verbindung erfolgreich \n");

                int offset = byteoffset * entry; //Berechnet den Offset eines Eintrags

                SPS.DBRead(25, offset, buffersize, buffer); //DB-Nummer, Start-Offset, Read-Size(bytes), Pointer auf Buffer

                EndianSwap16(ref buffer);

                for (int i = 0;  i < 31; i++)
                {
                    X_Koor[i] = BitConverter.ToInt16(buffer, 2*i); //X-Koor füllen
                    Y_Koor[i] = BitConverter.ToInt16(buffer, 2*i + 62); //Y-Koor füllen
                }
                Count = BitConverter.ToInt16(buffer, 124); // Count füllen
            }
            else
            {
                Console.WriteLine("\nVerbindungsfehler!\n");
            }
        }

        public void DebugRead() //Erzeugt Pseudozufallszahlen und schreibt sie in die Koordianten Arrays.
        {
            Random rng = new Random();
            Array.Clear(X_Koor, 0, arraysize); //
            Array.Clear(Y_Koor, 0, arraysize); //

            int entries = rng.Next(5, arraysize + 1); //Zahl zwischen 5 und arraysize(31)

            for (int i = 0; i < entries; i++)
            {
                X_Koor[i] = (short)rng.Next(100, 900);
                Y_Koor[i] = (short)rng.Next(100, 900);
            }
        }

        public void DebugWrite()
        {
            Array.Copy(w_X_Koor, 0, X_Koor, 0, arraysize);
            Array.Copy(w_Y_Koor, 0, Y_Koor, 0, arraysize);
        }

        public void WriteDBentry(int entry)
        {
            int offset = byteoffset * entry;

            for (int i = 0; i < 31; i++)
            {
                Array.Copy(BitConverter.GetBytes(X_Koor[i]), 0, writebuffer, i * 2, 2);
                Array.Copy(BitConverter.GetBytes(Y_Koor[i]), 0, writebuffer, i * 2 + 62, 2);
            }
            EndianSwap16(ref writebuffer);
            SPS.DBWrite(25, offset, buffersize - 2, writebuffer);

        }
        public bool IsConnected => connection;

        public void DestroyConnection() => SPS = null;
        public void SetX(short value, int index) => w_X_Koor[index] = value;
        public void SetY(short value, int index) => w_Y_Koor[index] = value;

        public short[] X => X_Koor;

        public short[] Y => Y_Koor;

        public short GetCount()
        {
            return Count;
        }

        public short GetXVal(int i) => X_Koor[i];
        public short GetYVal(int i) => Y_Koor[i];

        public void Print()
        {
            if (this.IsConnected)
            {
                Console.WriteLine("Einträge: {0} \n\nX-Koordianten:\n", Count);
                Array.ForEach(X_Koor, Int16 => Console.Write("{0} ", Int16));
                Console.WriteLine("\n\nY-Koordinaten:");
                Array.ForEach(Y_Koor, Int16 => Console.Write("{0} ", Int16));
                Console.WriteLine("\n\nEnde");
            }
            else
            {
                Console.WriteLine("Ausgabe konnte nicht erstellt werden, keine Verbindung zur Lesequelle!\n");
            }
        }

        public void EndianSwap16(ref byte[] swap) //Führt die Konversion Big -> Small Endian für shorts(16bit Integer) durch.
        {
            byte temp;
            for (int i = 0; i < buffersize - 2; i += 2)
            {
                temp = swap[i];
                swap[i] = swap[i + 1];
                swap[i + 1] = temp;
            }
        }
    }
}
