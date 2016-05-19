using Nektra.Deviare2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


class SectionSearch
{


    private NktProcess p;
    private bool searchDotTextSection;
    private Object fileLock = new Object();


    public SectionSearch(NktProcess p, bool searchDotTextSection)
    {
        this.p = p;
        this.searchDotTextSection = searchDotTextSection;
    }

    private void setDeepSearch()
    {
        searchDotTextSection = true;
    }

    //Searches for the query string in the process memory, written to a file
    public bool containsString(string query, bool refresh)
    {
        string filename = p.Id.ToString() + ".mca";
        lock (fileLock)
        {
            if (refresh || !File.Exists(filename))
            {
                getSections();
            }
            bool foundA = File.ReadAllText(filename, Encoding.ASCII).Contains(query, StringComparison.OrdinalIgnoreCase);
            bool foundU = File.ReadAllText(filename, Encoding.Unicode).Contains(query, StringComparison.OrdinalIgnoreCase);

            return foundA || foundU;
        }
    }
    //Creates a file with the process memory (Does NOT handle deleting, take care of it elsewhere!)
    private void getSections()
    {
        string filename = p.Id.ToString() + ".mca";
        BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.Create));

        //Write each section contents to file
        for (int i = 0; i < p.Sections().Count; i++)
        {
            NktStructPESections n = p.Sections();
            //Skip the .text section unless searchDotTextSection is enabled (true)
            if (n.Name[i] != ".text" || searchDotTextSection)
            {
                //Get start/end/size of section
                IntPtr sptr = n.StartAddress[i];
                int siptr = sptr.ToInt32();
                IntPtr eptr = n.EndAddress[i];
                int eiptr = eptr.ToInt32();
                int sectionsize = eiptr - siptr;
                //Alloc buffer to receive memory from API
                byte[] buffer = new byte[sectionsize];
                GCHandle pinnedArray = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                IntPtr pBuffer = pinnedArray.AddrOfPinnedObject();
                //Read section
                p.Memory().ReadMem(pBuffer, n.StartAddress[i], new IntPtr(sectionsize));
                //Free the pointer
                pinnedArray.Free();
                //Output to file
                bw.Write(buffer);
            }
        }
        bw.Close();
    }
}

