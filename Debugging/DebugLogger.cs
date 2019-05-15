using System.IO;
using UnityEngine;

public static class DebugLogger {

    public static bool enableDebugging = false;
    private static string m_Path;
    private static string m_InitMessage;
    private static string m_FileName;
    private static bool m_Initialized = false;

    private static string m_FullPath;

    public static void Init()
    {
        if(!m_Initialized)
        {
            m_Path = @"LogFiles\"; ;
            m_InitMessage = ("Log Created @ [" + System.DateTime.Now.ToString() + "]\r\n"); ;
            m_FileName = ("[" + System.DateTime.Now.ToString("MM-dd-yy") + "].log");
            m_FullPath = m_Path + m_FileName;
            m_Initialized = true;
        }
    }

    public static bool Log(string message)
    {
        if(enableDebugging)
        {

            if (Directory.Exists(m_Path))
            {

                try
                {
                    File.ReadAllText(m_FullPath);
                }
                catch
                {
                    File.AppendAllText(m_FullPath, m_InitMessage);
                }
            }
            else
            {

                Directory.CreateDirectory(m_Path);

                try
                {
                    File.ReadAllText(m_FullPath);
                }
                catch
                {
                    File.AppendAllText(m_FullPath, m_InitMessage);
                }


            }

            try
            {
                File.AppendAllText(m_FullPath, message);
                return true;
            }
            catch
            {
                return false;
            }
        }
        else { return false; }
        
    }

  
}
