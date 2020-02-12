/*
 * Autor: Jonathan Siems
 * 
 * Diese Klasse bietet Methoden zum Einlesen aus einer SPS an, die Variablen ausschliesslich in 16 Bit Integer hinterlegt.
 * 
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp7;

namespace SPSCtrl
{
    public class SPSBackend
    {
        public const int buffersize = 128; //Buffergröße in Bytes (Muss mit der Größe des Blocks übereinstimmen, der ausgelesen wird)
        public const int livebuffersize = 59;
        public const int byteoffset = 126; //Offset zwischen einzelnen Einträgen (X: 0-61, Y: 62-123, Einträge: 124-126

        private static S7Client SPS;
        public static int xylist_size = 31;
        private readonly bool isConnected;

        private short count;
        private short[] X_Koor, Y_Koor;

        private short[] w_X_Koor;
        private short[] w_Y_Koor;

        public short X_Live_Links, X_Live_Rechts; //Koordinaten der Düsen
        public short Y_Live_Links, Y_Live_Rechts;
        public short lastCycleTime;
        public short lastCleanTimeL;
        public short lastCleanTimeR;
        public short hersteller;
        public short pallette;
        public bool unNummer;
		public bool rastbar;
        public short schildGroesse_Links, schildGroesse_Rechts; // Vorderseite, Hinterseite
        public short state;

        private byte[] buffer = new byte[buffersize];
        private byte[] writebuffer = new byte[buffersize];
        private byte[] livebuffer = new byte[livebuffersize];

        private double xll, yll, xlr, ylr;


        public SPSBackend(string ip) //Konstruktor
        {
            SPS = new S7Client(); //Neue Verbindung erzeugen
            SPS.ConnectTo(ip, 0, 0); //IP_Adresse, Rack, Slot
            isConnected = SPS.Connected ? true : false;

            X_Koor = new short[xylist_size];
            Y_Koor = new short[xylist_size];
            w_X_Koor = new short[xylist_size];
            w_Y_Koor = new short[xylist_size];

        }

        ~SPSBackend() //Destruktor, trennt die Verbindung wenn Programm geschlossen wird.
        {
            SPS.Disconnect();
        }
        /// <summary>
        /// Liest die Koordinatenlisten aus der SPS ein. Errechnet entsprechende Position aus dem fest eingestellten Offset und dem eingegebenen Index.
        /// </summary>
        public bool ReadDBEntry(int entry)
        {
            if (isConnected) //Verbindung erfolgreich
            {
                int offset = byteoffset * entry; //Berechnet den absoluten Offset eines Eintrags

                SPS.DBRead(25, offset, buffersize, buffer); //DB-Nummer, Start-Offset, Read-Size(bytes), Pointer auf Buffer

                EndianSwap16(ref buffer, buffersize);

                for (int i = 0; i < 31; i++)
                {
                    X_Koor[i] = BitConverter.ToInt16(buffer, 2 * i); //X-Koor füllen
                    Y_Koor[i] = BitConverter.ToInt16(buffer, 2 * i + 62); //Y-Koor füllen
                }
                count = BitConverter.ToInt16(buffer, 124); // Count füllen

                return true;
            }
            else //Verbindung fehlgeschlagen
            {
                return false;
            }
        }

        /// <summary>
        /// Schreibt die zuvor in den Schreib-Arrays abgelegten Werte zurück in die SPS. Leert anschliessend die Schreib-Arrays
        /// </summary>

        public int WriteDBentry(int entry)
        {
            int offset = byteoffset * entry;
            short cnt = 0;

            for (int i = 0; i < 31; i++)
            {
                Array.Copy(BitConverter.GetBytes(w_X_Koor[i]), 0, writebuffer, i * 2, 2); //X-Koordianten: Byte (0, 62]
                Array.Copy(BitConverter.GetBytes(w_Y_Koor[i]), 0, writebuffer, i * 2 + 62, 2); //Y-Koordianten: Byte (62, 124]

                if (w_X_Koor[i] > 0 || w_Y_Koor[i] > 0) //Zählt Einträge > 0;
                    cnt++;
            }
            Array.Copy(BitConverter.GetBytes(cnt), 0, writebuffer, 124, 2);//Count: Byte (124,125)

            EndianSwap16(ref writebuffer, buffersize);

            Array.Clear(w_X_Koor, 0, xylist_size);//Write-Arrays leeren
            Array.Clear(w_Y_Koor, 0, xylist_size);

            return SPS.DBWrite(25, offset, buffersize - 2, writebuffer); //Schreibt den writebuffer zurück auf die SPS und gibt bei Erfolg true zurück
        }

        /// <summary>
        /// Liest Live-Daten über die Düsenposition etc. aus der SPS.
        /// </summary>
        public void ReadLiveData()
        {
            SPS.DBRead(4, 0, livebuffersize, livebuffer);
            EndianSwap16(ref livebuffer, livebuffersize);

            xll = BitConverter.ToDouble(livebuffer, 0);
            yll = BitConverter.ToDouble(livebuffer, 2);
            xlr = BitConverter.ToDouble(livebuffer, 4);
            ylr = BitConverter.ToDouble(livebuffer, 6);

            X_Live_Links = BitConverter.ToInt16(livebuffer, 0);
            Y_Live_Links = BitConverter.ToInt16(livebuffer, 2);
            X_Live_Rechts = BitConverter.ToInt16(livebuffer, 4);
            Y_Live_Rechts = BitConverter.ToInt16(livebuffer, 6);
            state = BitConverter.ToInt16(livebuffer, 18);
            lastCycleTime = BitConverter.ToInt16(livebuffer, 26);
            lastCleanTimeL = BitConverter.ToInt16(livebuffer, 42);
            lastCleanTimeR = BitConverter.ToInt16(livebuffer, 44);
            hersteller = BitConverter.ToInt16(livebuffer, 46);
            pallette = BitConverter.ToInt16(livebuffer, 48);
            unNummer = BitConverter.ToBoolean(livebuffer, 51);
            schildGroesse_Links = (short)livebuffer[52];
            schildGroesse_Rechts = (short)livebuffer[54];
			rastbar = BitConverter.ToBoolean(livebuffer, 58);

        }

        public bool IsConnected => isConnected;

        /// <summary>
        /// Schreibt einen einzelnen X-Wert an Index in das zu-schreibende Array.
        /// </summary>
        public void WriteX(short value, int index) => w_X_Koor[index] = value;
        /// <summary>
        /// Schreibt einen einzelnen Y-Wert an Index in das zu-schreibende Array.
        /// </summary>
        public void WriteY(short value, int index) => w_Y_Koor[index] = value;

        /// <summary>
        /// Gibt ein Byte-Array mit den aktuellen X-Koordinaten zurück
        /// </summary>
        public short[] X => X_Koor;

        /// <summary>
        /// Gibt ein Byte-Array mit den aktuellen Y-Koordinaten zurück.
        /// </summary>
        public short[] Y => Y_Koor; //Gibt das Y-Array zurück

        /// <summary>
        ///Gibt die aktuelle Anzahl aller Einträge >0 zurück.
        /// </summary>
        public short CountXY => count; // Gibt Anzahl der Einträge zurück

        /// <summary>
        /// Gibt X-Wert am gegebenen Index zurück.
        /// </summary>
        public short GetXVal(int i) => X_Koor[i]; //Gibt einen bestimmten X Wert

        /// <summary>
        /// Gibt Y-Wert am gegebenen Index zurück.
        /// </summary>
        public short GetYVal(int i) => Y_Koor[i]; // Gibt einen bestimmten Y Wert

        /// <summary>
        /// Führt die Konversion Big -> Small Endian für shorts(16bit Integer) durch. 
        /// <para>Args: Byte-Array-Referenz, Byte-Array-Größe.</para>
        /// </summary>
        public static void EndianSwap16(ref byte[] swap, int size)
        {
            byte temp;
            for (int i = 0; i < size - 2; i += 2)
            {
                temp = swap[i];
                swap[i] = swap[i + 1];
                swap[i + 1] = temp;
            }
        }
    }
}
