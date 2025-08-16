namespace trybe_hotel.Test.Test;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using trybe_hotel;
using System.Diagnostics;
using System.Xml;
using System.IO;


public class TestReq09
{
    [Fact]
    [Trait("Category", "9. Desenvolva testes que cubram no mínimo 40% de linhas")]
    public static void Testes()
    {
        string pathXml = System.Environment.CurrentDirectory.ToString().Replace("TrybeHotel.Test.Test/bin/Debug/net6.0","") + "/TrybeHotel.Test/TestResults/";
        try {
            Directory.Delete(pathXml, true);
        } catch(Exception ex) {}

        ProcessStartInfo startInfo = new ProcessStartInfo()
        {
            FileName = System.Environment.CurrentDirectory.ToString().Replace("bin/Debug/net6.0","") + "coverlet.sh",
            WorkingDirectory = System.Environment.CurrentDirectory.ToString().Replace("bin/Debug/net6.0",""),
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };
        Process proc = new Process()
        {
            StartInfo = startInfo
        };
        proc.Start();
        /*string output = proc.StandardOutput.ReadToEnd();
        System.Console.WriteLine(output);
        string err = proc.StandardError.ReadToEnd();
        System.Console.WriteLine(err);
        */
        proc.WaitForExit();
        System.Console.WriteLine("Process coverlet status: " + proc.ExitCode+ " - " + proc.HasExited);

        string[] dirs = Directory.GetDirectories(pathXml);
        pathXml = dirs[0] + "/coverage.cobertura.xml";

        System.Console.WriteLine("pathXml:" + pathXml);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(pathXml);
        string lines_covered = xmlDoc.SelectSingleNode("coverage").Attributes["line-rate"].Value;
        
        double lines_covered_rate = double.Parse(lines_covered);
        System.Console.WriteLine("cobertura de testes: " + lines_covered_rate.ToString() );
        Assert.True(lines_covered_rate >= 0.4, "Desenvolva testes que cubram no mínimo 40% de linhas");

        pathXml = System.Environment.CurrentDirectory.ToString().Replace("TrybeHotel.Test.Test/bin/Debug/net6.0","") + "/TrybeHotel.Test/TestResults/";
        //Directory.Delete(pathXml, true);
    }
}