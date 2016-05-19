
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

public class StringScan
{

    // REQUIRED CONSTS

    const int PROCESS_QUERY_INFORMATION = 0x0400;
    const int MEM_COMMIT = 0x00001000;
    const int PAGE_READWRITE = 0x04;
    const int PROCESS_VM_READ = 0x0010;


    // REQUIRED METHODS

    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

    [DllImport("kernel32.dll")]
    static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

    [DllImport("msvcrt.dll")]
    private static extern int memcmp(byte[] b1, byte[] b2, int count);


    // REQUIRED STRUCTS

    public struct MEMORY_BASIC_INFORMATION
    {
        public int BaseAddress;
        public int AllocationBase;
        public int AllocationProtect;
        public int RegionSize;
        public int State;
        public int Protect;
        public int lType;
    }

    public struct SYSTEM_INFO
    {
        public ushort processorArchitecture;
        ushort reserved;
        public uint pageSize;
        public IntPtr minimumApplicationAddress;
        public IntPtr maximumApplicationAddress;
        public IntPtr activeProcessorMask;
        public uint numberOfProcessors;
        public uint processorType;
        public uint allocationGranularity;
        public ushort processorLevel;
        public ushort processorRevision;
    }


    public static void Resize<T>(this List<T> list, int sz, T c)
    {
        int cur = list.Count;
        if (sz < cur)
            list.RemoveRange(sz, cur - sz);
        else if (sz > cur)
        {
            if (sz > list.Capacity)//this bit is purely an optimisation, to avoid multiple automatic capacity changes.
                list.Capacity = sz;
            list.AddRange(Enumerable.Repeat(c, sz - cur));
        }
    }
    public static void Resize<T>(this List<T> list, int sz) where T : new()
    {
        Resize(list, sz, new T());
    }

    IntPtr GetAddressOfData(int pid, char[] data, int len)
    {
        Process process = Process.GetProcessById(pid);
        IntPtr hProcess = OpenProcess(PROCESS_VM_READ | PROCESS_QUERY_INFORMATION, false, process.Id);

        SYSTEM_INFO sys_info = new SYSTEM_INFO();
        GetSystemInfo(out sys_info);

        // this will store any information we get from VirtualQueryEx()
        MEMORY_BASIC_INFORMATION mem_info = new MEMORY_BASIC_INFORMATION();

        IntPtr proc_min_address = sys_info.minimumApplicationAddress;
        IntPtr proc_max_address = sys_info.maximumApplicationAddress;

        // saving the values as long ints so I won't have to do a lot of casts later
        long proc_min_address_l = (long)proc_min_address;
        long proc_max_address_l = (long)proc_max_address;

        IntPtr p = new IntPtr(mem_info.BaseAddress);
        while (proc_min_address_l < proc_max_address_l)
        {
            VirtualQueryEx(hProcess, p, out mem_info, 28);

            byte[] chunk = new byte[mem_info.RegionSize];
            int bytesRead = 0;
            if (ReadProcessMemory((int)hProcess, p, chunk, mem_info.RegionSize, ref bytesRead))
            {
                for (int i = 0; i < (bytesRead - len); ++i)
                {
                    if (memcmp(data, chunk, len) == 0)
                    {
                        return (char*)p + i;
                    }
                }
            }
            p += mem_info.RegionSize;

        }
        return new IntPtr(0);
    }

    void find(int pid)
    {

        const char[] someData = "SomeDataToFind";
        std::cout << "Local data address: " << (void*)someData << "\n";


        IntPtr ret = GetAddressOfData(pid, someData, Marshal.SizeOf(someData));
        if ((int)ret>0)
        {
            Debug.WriteLine("found");
        }
        else
        {
            Debug.WriteLine("not found");
        }

    }
}
