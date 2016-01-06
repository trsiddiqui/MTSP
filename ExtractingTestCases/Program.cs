using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HtmlAgilityPack;
using edu.stanford.nlp.tagger.maxent;
using java.util;
using System.Diagnostics;
using System.Data.OleDb;
using System.Data;
using OfficeOpenXml;
using System.Xml.Serialization;

namespace ExtractingTestCases
{
    public class TestCase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Steps { get; set; }
        public string Version { get; set; }
        public string Product { get; set; }
    }
    public class TestResult
    {
        public int Id { get; set; }
        public bool Result { get; set; }
        public string Version { get; set; }
        public string Product { get; set; }
    }
    class Program
    {
        #region constants
        ////LITMUS 10
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_10\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_10_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_10_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_10\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_10_UniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_10_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_10_TestIdWithUniquePair.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_10_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_10_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_10_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_10_ResultForCharts_APFD_.txt";



        ////LITMUS 11
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_11\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_11_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_11_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_11\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_11_UniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_11_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_11_TestIdWithUniquePair.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_11_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_11_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_11_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_11_ResultForCharts_APFD_.txt";
        ////LITMUS 12
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_12\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_12_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_12_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_12\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_12_UniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_12_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_12_TestIdWithUniquePair.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_12_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_12_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_12_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_12_ResultForCharts_APFD_.txt";

        ////LITMUS 13
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_13\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_13_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_13_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_13\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_13_UniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_13_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_13_TestIdWithUniquePair.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_13_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_13_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_13_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_13_ResultForCharts_APFD_.txt";

        ////LITMUS 14
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_14\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_14_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_14_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_14\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_14_UniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_14_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_14_TestIdWithUniquePair.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_14_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_14_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_14_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_14_ResultForCharts_APFD_.txt";

        ////LITMUS 40
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_40\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_40_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_40_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_40\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_40_UniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_40_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_40_TestIdWithUniquePair.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_40_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_40_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_40_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_40_ResultForCharts_APFD_.txt";
        ////LITMUS 50
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_50\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_50\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_UniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_TestIdWithUniquePair.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_50_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_50_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_50_ResultForCharts_APFD_.txt";
        ////LITMUS 60
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_60\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_60_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_60_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_60\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_60_UniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_60_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_60_TestIdWithUniquePair.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_60_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_60_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_60_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_60_ResultForCharts_APFD_.txt";
        ////LITMUS 70
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_70\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_70_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_70_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_70\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_70_UniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_70_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_70_TestIdWithUniquePair.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_70_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_70_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_70_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_70_ResultForCharts_APFD_.txt";
        //LITMUS 80
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_80\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_80_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_80_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_80\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_80_UniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_80_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_80_TestIdWithUniquePair.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_80_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_80_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_80_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_80_ResultForCharts_APFD_.txt";
        ////LITMUS 90
        const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_90\litmus.mozilla.org";
        const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_90_Rapid.txt";
        const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_90_Rapid-Tagged.txt";
        const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_90\TestResults.txt";
        const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_90_UniquePair.txt";
        const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_90_UniqueMultiplets.txt";
        const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_90_TestIdWithUniquePair.txt";
        const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_90_TestIdWithUniqueMultiplets.txt";
        const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_90_TestIdWithTopicCounts.txt";
        const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_90_ResultForCharts_APFD_.txt";
        const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_90_ResultForCharts_APFD_.txt";

        #endregion

        const string excelPathForTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\TestSteps.xlsx";
        const string excelPathForTestCasesResults = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\TestResults.xlsx";
        const string directoryForExtractedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\LatestVersions";

        const double NAPFDConstant = 1;
        static void Main(string[] args)
        {
            var program = new Program();
            bool interactive = true;
            //program.QueryExcelForNewVersions();
            //program.ProcessSerializedTestCases();
            //program.MakeNounVerbPairsForExcel(interactive);
            //program.PrepareResultsFromExcel();
            //program.OrderForNounVerbPairExcel(interactive);

            //program.ExtractTestCases(interactive);
            //program.MakeNounVerbPairs(interactive);
            //program.OrderForNounVerbPair(interactive); //FindIfTestCasesMissing(interactive);

            //program.MakeNounVerbMultiplets(interactive);            
            program.CountNounAndVerbCombined(interactive);
            program.OrderForTopicCounts(interactive);

        }

        public void PrepareResultsFromExcel()
        {

            List<TestResult> testResults = new List<TestResult>();
            byte[] file = File.ReadAllBytes(excelPathForTestCasesResults);
            MemoryStream ms = new MemoryStream(file);
            using (ExcelPackage package = new ExcelPackage(ms))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                var rowCount = worksheet.Cells.Count() / 5 /*Column Count*/;
                for (var a = 2 /*Skipping header*/; a <= rowCount; a++)
                {
                    var product = (worksheet.Cells[a, 1].Value).ToString();
                    var version = (worksheet.Cells[a, 2].Value).ToString();
                    var id = Convert.ToInt32(worksheet.Cells[a, 3].Value);
                    var result = (worksheet.Cells[a, 5].Value).ToString();
                    if (testResults.Any(x => x.Id == id))
                    {
                        var previousResult = testResults.FirstOrDefault(x => x.Id == id).Result;
                        testResults.FirstOrDefault(x => x.Id == id).Result = previousResult && result == "passed";
                    }
                    else
                    {
                        testResults.Add(new TestResult() { Id = id, Product = product, Result = result == "passed", Version = version });
                    }
                    Console.Out.WriteLine("Progress: " + (float)a / rowCount * 100 + "%");
                }
            }
            string obj = this.SerializeResults(testResults);
            if (!File.Exists(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\LatestVersions\ExtractedObjectForAllTestResults.txt"))
            {
                File.Create(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\LatestVersions\ExtractedObjectForAllTestResults.txt").Close();
            }
            File.WriteAllText(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\LatestVersions\ExtractedObjectForAllTestResults.txt", obj);
        }

        public void QueryExcelForNewVersions()
        {
            List<TestCase> testCases = new List<TestCase>();
            byte[] file = File.ReadAllBytes(excelPathForTestCases);
            MemoryStream ms = new MemoryStream(file);
            using (ExcelPackage package = new ExcelPackage(ms))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                var rowCount = worksheet.Cells.Count() / 5 /*Column Count*/;
                for (var a = 2 /*Skipping header*/; a <= rowCount; a++)
                {
                    var test = new TestCase();
                    string version = worksheet.Cells[a, 2].Value.ToString();
                    int caseId = Convert.ToInt32(worksheet.Cells[a, 3].Value);
                    if (testCases.Any(x => x.Id == caseId))
                    {
                        object step = worksheet.Cells[a, 5].Value;
                        if (step != null)
                            testCases.FirstOrDefault(x => x.Id == caseId).Steps.Add(step.ToString());
                    }
                    else
                    {
                        string caseName = worksheet.Cells[a, 4].Value.ToString();
                        object step = worksheet.Cells[a, 5].Value;
                        string product = worksheet.Cells[a, 1].Value.ToString();
                        test.Id = caseId;
                        test.Name = caseName;
                        if (step != null)
                            test.Steps = new List<string>() { step.ToString() };
                        else
                            test.Steps = new List<string>();
                        test.Version = version;
                        test.Product = product;
                        testCases.Add(test);
                    }
                    Console.Out.WriteLine((double)a / rowCount * 100);
                }
            }
            string obj = this.SerializeObject(testCases);
            if (!File.Exists(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\LatestVersions\ExtractedObjectForAllTestCases.txt"))
            {
                File.Create(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\LatestVersions\ExtractedObjectForAllTestCases.txt");
            }
            File.WriteAllText(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\LatestVersions\ExtractedObjectForAllTestCases.txt", obj);
        }

        public string SerializeObject(List<TestCase> toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<TestCase>));

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
        public string SerializeResults(List<TestResult> toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<TestResult>));

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
        public List<TestCase> DeserializeObject(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<TestCase>));

            StreamReader reader = new StreamReader(path);
            var testCases = (List<TestCase>)xmlSerializer.Deserialize(reader);
            reader.Close();
            return testCases;
        }
        public List<TestResult> DeserializeResults(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<TestResult>));

            StreamReader reader = new StreamReader(path);
            var testResults = (List<TestResult>)xmlSerializer.Deserialize(reader);
            reader.Close();
            return testResults;
        }
        public string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == ' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        public void ProcessSerializedTestCases()
        {
            var testCases = new List<TestCase>();
            testCases = DeserializeObject(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\LatestVersions\ExtractedObjectForAllTestCases.txt");




            var tagger = new MaxentTagger(taggerModelPath);
            //testcases object now contains all the test cases
            var productVersions = (from t in testCases select t.Product + "~" + t.Version).Distinct();
            var count = 0;
            foreach (var version in productVersions)
            {
                StringBuilder allFiles = new StringBuilder();
                StringBuilder allTaggedFiles = new StringBuilder();
                count++;
                var testCasesForThisVersion = testCases.Where(x => x.Product == version.Split('~')[0] && x.Version == version.Split('~')[1]);
                var countTestCases = 0;
                foreach (var testCase in testCasesForThisVersion)
                {
                    Console.Out.Write((double)count / productVersions.Count() * 100 + "%. Version #" + count + ":" + version + " out of " + productVersions.Count() + ". ");
                    Console.Out.Write((double)countTestCases / testCasesForThisVersion.Count() * 100 + "%. TestCase " + countTestCases++ + " out of " + testCasesForThisVersion.Count() + "\n");
                    allFiles.Append("Test Case ID: " + testCase.Id + "~" + testCase.Product + testCase.Version + "\n");
                    allTaggedFiles.Append("Test Case ID: " + testCase.Id + "~" + testCase.Product + testCase.Version + "\n");


                    StringBuilder steps = new StringBuilder();
                    StringBuilder taggedSteps = new StringBuilder();

                    foreach (var step in testCase.Steps)
                    {
                        steps.Append(RemoveSpecialCharacters(step) + " \n");
                        var sentences = MaxentTagger.tokenizeText(new java.io.StringReader(RemoveSpecialCharacters(step))).toArray();
                        foreach (ArrayList sentence in sentences)
                        {
                            var tagged = tagger.tagSentence(sentence).ToString();
                            //ONLY FETCH NOUN AND VERB, DISCARD OTHERS
                            taggedSteps.Append(FetchNounAndVerbsOnly(tagged.Substring(1, tagged.Length - 2)) + "\n");
                        }
                    }

                    //appending 3 lines after each test case
                    allFiles.Append(steps + "\n \n \n");
                    //appending 3 lines after each tagged test case
                    allTaggedFiles.Append(taggedSteps + "\n \n \n");
                }
                if (!File.Exists(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\LatestVersions\Raw" + version + ".txt"))
                {
                    File.Create(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\LatestVersions\Raw" + version + ".txt").Close();
                    File.Create(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\LatestVersions\Tagged" + version + ".txt").Close();
                }
                File.WriteAllText(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\LatestVersions\Raw" + version + ".txt", allFiles.ToString());
                File.WriteAllText(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\LatestVersions\Tagged" + version + ".txt", allTaggedFiles.ToString());
            }
        }
        public void ExtractTestCases(bool interactive)
        {

            // Loading POS Tagger
            var tagger = new MaxentTagger(taggerModelPath);
            StringBuilder allFiles = new StringBuilder();
            StringBuilder allTaggedFiles = new StringBuilder();
            var files = Directory.EnumerateFiles(folderToLookForTestCases, "*.*");
            var filesProcessed = 0.0;
            var fileCount = files.Count();
            foreach (string file in files)
            {
                //if (interactive)
                Console.Out.WriteLine((float)filesProcessed++ / (float)fileCount * 100 + " % ");
                //OLD:Fetching only Page 0 if multiple pages of a test case exist, same steps in all
                //NEW:Fetching Single_Result files only, because we only have results for those
                if (file.Contains("single_result") && file.Contains("id=") && !file.Contains("&page="))
                {

                    string contents = File.ReadAllText(file);
                    string[] specialCharacters = { "&quot;", "&#139;", "&#39;", "&lt;", "&#60;", "&gt;", "&#62;", "&amp;", "&#38;", "&cent;", "&#162;", "&pound;", "&#163;", "&yen;", "&#165;", "&euro;", "&#8364;", "&copy;", "&#169;", "&reg;", "&#174" };
                    foreach (var str in specialCharacters)
                    {
                        contents = contents.Replace(@str, "");

                    }
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(contents);

                    // if Testcase Disabled, dont care
                    if (doc.DocumentNode.SelectSingleNode("//head/title").InnerText.ToLower().Contains("testcase disabled"))
                    {
                        continue;
                    }

                    //Test cases in all files are enclosed in a div with class name "dv", hence fetching that div
                    var stepHtml = doc.DocumentNode.SelectSingleNode(".//div[@class='dv']");
                    StringBuilder steps = new StringBuilder();
                    StringBuilder taggedSteps = new StringBuilder();
                    if (stepHtml != null)
                    {
                        //Appending test case ID to RAW test cases file
                        allFiles.Append("Test Case ID: " + file.Substring(file.LastIndexOf("id") + 3) + "~" + file + "\n");

                        //Appending test case ID to tagged test cases file
                        allTaggedFiles.Append("Test Case ID: " + file.Substring(file.LastIndexOf("id") + 3) + "~" + file + "\n");
                        var secDoc = new HtmlDocument();
                        secDoc.LoadHtml(stepHtml.InnerHtml);
                        //Since the steps are written in paragraph tags .......
                        if (stepHtml.InnerHtml.Contains("<p>"))
                        {
                            foreach (var node in secDoc.DocumentNode.SelectNodes(".//p"))
                            {
                                steps.Append(node.InnerText + " \n");
                                var sentences = MaxentTagger.tokenizeText(new java.io.StringReader(node.InnerText)).toArray();
                                foreach (ArrayList sentence in sentences)
                                {
                                    var tagged = tagger.tagSentence(sentence).ToString();
                                    //ONLY FETCH NOUN AND VERB, DISCARD OTHERS
                                    taggedSteps.Append(FetchNounAndVerbsOnly(tagged.Substring(1, tagged.Length - 2)) + "\n");
                                }
                            }
                        }
                        //Or the steps are written in unordered list
                        if (stepHtml.InnerHtml.Contains("<li>"))
                        {
                            foreach (var node in secDoc.DocumentNode.SelectNodes(".//li"))
                            {
                                steps.Append(node.InnerText + " \n");
                                var sentences = MaxentTagger.tokenizeText(new java.io.StringReader(node.InnerText)).toArray();
                                foreach (ArrayList sentence in sentences)
                                {
                                    var tagged = tagger.tagSentence(sentence).ToString();
                                    //ONLY FETCH NOUN AND VERB, DISCARD OTHERS
                                    taggedSteps.Append(FetchNounAndVerbsOnly(tagged.Substring(1, tagged.Length - 2)) + "\n");
                                }
                            }
                        }
                    }
                    //appending 3 lines after each test case
                    allFiles.Append(steps + "\n \n \n");
                    //appending 3 lines after each tagged test case
                    allTaggedFiles.Append(taggedSteps + "\n \n \n");

                    if (interactive)
                    {
                        Console.WriteLine(steps);
                        Console.WriteLine(taggedSteps);
                    }
                }
            }
            //Saving test cases
            File.WriteAllText(fileToSaveExtractedRawTestCases, allFiles.ToString());
            File.WriteAllText(fileToSaveExtractedAndTaggedTestCases, allTaggedFiles.ToString());
        }
        #region Helper Method
        private string FetchNounAndVerbsOnly(string v)
        {

            string[] allowedTags = { "NN", "NNS", "NNP", "NNPS", "VV", "VB", "VBD", "VBG", "VBN", "VBP", "VBZ" };
            List<string> finalString = new List<string>();
            foreach (var taggedWord in v.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (taggedWord.Contains("/") && (allowedTags.Contains(taggedWord.Split('/')[1]) || taggedWord.Split('/')[0].ToString().ToLower() == "select") && taggedWord.Split('/')[0].ToString().ToLower() != "|")
                {
                    if (taggedWord.Split('/')[0].ToString().ToLower() == "select")
                        finalString.Add("Select/VB");
                    else
                        finalString.Add(taggedWord);
                }
            }
            return string.Join(" ", finalString);
        }
        #endregion

        #region NounVerbPairs
        public void MakeNounVerbPairs(bool interactive)
        {
            string[] verbTags = { "VV", "VB", "VBD", "VBG", "VBN", "VBP", "VBZ" };
            var contents = File.ReadAllText(fileToSaveExtractedAndTaggedTestCases);
            var finalString = new StringBuilder();
            int currentTestCaseID = -1;
            var allPairs = new List<string>();
            var testCase_NounVerbPair = new List<KeyValuePair<int, List<string>>>();
            var totalLines = contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Count();
            var passedLines = 0;
            //Traversing through lines in tagged file
            foreach (var line in contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {

                var currentTestCaseHeader = "";
                //IF line is not starting of a test case
                if (!line.Contains("Test Case ID: "))
                {
                    string previousTag = "";
                    string previousWord = "";
                    foreach (var taggedWord in line.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Contains('/')))
                    {
                        string currentWord = taggedWord.Split('/')[0];
                        string currentTag = taggedWord.Split('/')[1];
                        //IF this word is a verb, then store the word as previous word (to be used as the first word in a pair)
                        if (verbTags.Contains(currentTag))
                        {
                            previousTag = currentTag;
                            previousWord = currentWord;
                        }
                        else
                        {
                            //ELSE it can only be a noun, so make a pair using the previous word, and add it in the list AllPairs and finalString
                            //AND also store it in the matrix (KeyValue pair of (testcase,List of pairs))
                            if (!string.IsNullOrEmpty(previousWord))
                            {
                                var pair = "(" + previousWord.ToLower() + "_" + currentWord.ToLower() + ")";
                                allPairs.Add(pair);
                                finalString.Append(pair);
                                testCase_NounVerbPair.FirstOrDefault(x => x.Key == currentTestCaseID).Value.Add(pair);
                            }
                        }
                    }
                }
                //IF line is starting of a test case, then store the current test case information
                else
                {
                    var temp = line.Split(new String[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[1].Split('~')[0];
                    if (temp.Contains("."))
                        currentTestCaseID = Convert.ToInt32(temp.Split('.')[0]);
                    else
                        currentTestCaseID = Convert.ToInt32(temp);
                    testCase_NounVerbPair.Add(new KeyValuePair<int, List<string>>(currentTestCaseID, new List<string>()));
                    currentTestCaseHeader = line;
                    finalString.Append(currentTestCaseHeader + "\n");
                }
                finalString.Append("\n");
                passedLines++;
                if (interactive)
                {
                    Console.Clear();
                    Console.Out.WriteLine((float)passedLines / (float)totalLines * 100 + "%");
                }
            }
            List<string> distinctPairs = new List<string>();
            distinctPairs.AddRange(allPairs.Distinct());
            //Save the pairs and testcase IDs with pairs
            WriteUniquePairsAndTestCasesWithPairs(distinctPairs, testCase_NounVerbPair, null);
        }
        public void MakeNounVerbPairsForExcel(bool interactive)
        {
            string[] verbTags = { "VV", "VB", "VBD", "VBG", "VBN", "VBP", "VBZ" };
            var files = Directory.GetFiles(directoryForExtractedTestCases).Where(x => x.Contains("Tagged"));
            var fileCount = 0;
            foreach (var file in files)
            {
                fileCount++;
                var contents = File.ReadAllText(file);
                var finalString = new StringBuilder();
                int currentTestCaseID = -1;
                var allPairs = new List<string>();
                var testCase_NounVerbPair = new List<KeyValuePair<int, List<string>>>();
                var totalLines = contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Count();
                var passedLines = 0;
                //Traversing through lines in tagged file
                foreach (var line in contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {

                    var currentTestCaseHeader = "";
                    //IF line is not starting of a test case
                    if (!line.Contains("Test Case ID: "))
                    {
                        string previousTag = "";
                        string previousWord = "";
                        foreach (var taggedWord in line.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Contains('/')))
                        {
                            string currentWord = taggedWord.Split('/')[0];
                            string currentTag = taggedWord.Split('/')[1];
                            //IF this word is a verb, then store the word as previous word (to be used as the first word in a pair)
                            if (verbTags.Contains(currentTag))
                            {
                                previousTag = currentTag;
                                previousWord = currentWord;
                            }
                            else
                            {
                                //ELSE it can only be a noun, so make a pair using the previous word, and add it in the list AllPairs and finalString
                                //AND also store it in the matrix (KeyValue pair of (testcase,List of pairs))
                                if (!string.IsNullOrEmpty(previousWord))
                                {
                                    var pair = "(" + previousWord.ToLower() + "_" + currentWord.ToLower() + ")";
                                    allPairs.Add(pair);
                                    finalString.Append(pair);
                                    testCase_NounVerbPair.FirstOrDefault(x => x.Key == currentTestCaseID).Value.Add(pair);
                                }
                                else
                                {
                                    previousTag = currentTag;
                                    previousWord = currentWord;
                                }
                            }
                        }
                    }
                    //IF line is starting of a test case, then store the current test case information
                    else
                    {
                        var temp = line.Split(new String[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[1].Split('~')[0];
                        if (temp.Contains("."))
                            currentTestCaseID = Convert.ToInt32(temp.Split('.')[0]);
                        else
                            currentTestCaseID = Convert.ToInt32(temp);
                        testCase_NounVerbPair.Add(new KeyValuePair<int, List<string>>(currentTestCaseID, new List<string>()));
                        currentTestCaseHeader = line;
                        finalString.Append(currentTestCaseHeader + "\n");
                    }
                    finalString.Append("\n");
                    passedLines++;
                    if (interactive)
                    {
                        Console.Write("File # " + fileCount + "/" + files.Count() + ". Progress: ");
                        Console.Out.WriteLine((float)passedLines / (float)totalLines * 100 + "%");
                    }
                }
                List<string> distinctPairs = new List<string>();
                distinctPairs.AddRange(allPairs.Distinct());
                //Save the pairs and testcase IDs with pairs
                WriteUniquePairsAndTestCasesWithPairs(distinctPairs, testCase_NounVerbPair, file);
            }
        }
        public void WriteUniquePairsAndTestCasesWithPairs(List<string> uniquePairs, List<KeyValuePair<int, List<string>>> testId_pair, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                File.WriteAllText(fileToSaveUniquePairs, string.Join(",", uniquePairs));
            else
                File.WriteAllText(fileName.Substring(0, fileName.LastIndexOf(".")) + "_UniquePairs.txt", string.Join(",", uniquePairs));
            StringBuilder fileString = new StringBuilder();
            foreach (var test in testId_pair)
            {
                fileString.Append("id=" + test.Key + "\n");
                fileString.Append(string.Join(",", test.Value));
                fileString.Append("\n");
            }
            if (string.IsNullOrEmpty(fileName))
                File.WriteAllText(fileToSaveTestIdWithUniquePairs, fileString.ToString());
            else
                File.WriteAllText(fileName.Substring(0, fileName.LastIndexOf(".")) + "_Pairs.txt", fileString.ToString());
        }
        public void ReadUniquePairsAndTestCasesWithPairs(out List<string> uniquePairs, out List<KeyValuePair<int, List<string>>> testId_pair, string path)
        {
            uniquePairs = new List<string>();
            testId_pair = new List<KeyValuePair<int, List<string>>>();
            var testIdPairsContent = "";
            if (string.IsNullOrEmpty(path))
            {
                uniquePairs.AddRange(File.ReadAllText(fileToSaveUniquePairs).Split(','));
                testIdPairsContent = File.ReadAllText(fileToSaveTestIdWithUniquePairs);
            }
            else
            {
                uniquePairs.AddRange(File.ReadAllText(path.Substring(0, path.LastIndexOf("_")) + "_UniquePairs.txt").Split(','));
                testIdPairsContent = File.ReadAllText(path.Substring(0, path.LastIndexOf("_")) + "_Pairs.txt");
            }
            var first = true;
            var previousId = 0;
            List<string> pairs = new List<string>();
            foreach (var line in testIdPairsContent.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {

                if (line.StartsWith("id="))
                {
                    if (first)
                        first = false;
                    else
                    {
                        testId_pair.Add(new KeyValuePair<int, List<string>>(previousId, pairs));
                    }
                    previousId = Convert.ToInt32(line.Split('=')[1]);
                    pairs = new List<string>();
                }
                else
                {
                    if (!string.IsNullOrEmpty(line))
                        pairs.AddRange(line.Split(','));
                }
            }
            if (!testId_pair.Any(x => x.Key == previousId))
            {
                testId_pair.Add(new KeyValuePair<int, List<string>>(previousId, pairs));
            }
        }
        public void OrderForNounVerbPair(bool interactive)
        {
            List<string> uniquePairs;
            List<KeyValuePair<int, List<string>>> testId_pair;
            ReadUniquePairsAndTestCasesWithPairs(out uniquePairs, out testId_pair, "");


            #region Counting Pair Support, Each pair with its count
            List<KeyValuePair<string, int>> pairCounts = new List<KeyValuePair<string, int>>();
            var iCount = 0.0;
            foreach (var pair in uniquePairs)
            {
                iCount++;
                //get count of test cases having this pair
                var count = (from tip in testId_pair
                             where tip.Value.Contains(pair)
                             select tip).Count();
                //storing each pair with its count
                pairCounts.Add(new KeyValuePair<string, int>(pair, count));
                Console.Clear();
                Console.Out.WriteLine("Pair Counting: " + iCount / uniquePairs.Count * 100 + "%");
            }
            //ordering pairs with their counts
            pairCounts = pairCounts.OrderByDescending(x => x.Value).ToList();
            #endregion

            #region Preparing Data for ordering
            //ordering the list of only pairs with the help of the list of pair with counts that was sorted in the previous step
            uniquePairs = (from u in pairCounts
                           join p in uniquePairs on u.Key equals p
                           select u.Key).ToList();

            //taking counts of Pairs with each test case
            var testId_countOfPair = (from p in testId_pair
                                      select new KeyValuePair<int, int>(p.Key, p.Value.Count)).ToList();
            //ordering Test cases by counts
            var sortedTestId_countOfPair = testId_countOfPair.OrderByDescending(x => x.Value).ToList();
            #endregion

            #region Extracting Test Results from the File
            //extracting the results of the test cases of this litmus
            /**********************************************************************/
            /******************************IMPORTANT*******************************/
            /********Go to the result file, and replace § character with ~*********/
            var resultFile = File.ReadAllLines(fileToExtractResultsOfTestCasesFrom);
            var totalFaults = 0;
            //record results in this list, and put true if a test case passes else false for fail or bad test case
            List<KeyValuePair<int, bool>> testResults = new List<KeyValuePair<int, bool>>();
            foreach (var line in resultFile)
            {
                if (line.Contains("id="))
                {
                    var testId = Convert.ToInt32(line.Substring(line.IndexOf("id=") + 3/*for i d =*/).Split('~')[0]);
                    if (line.Substring(0, 10).Contains("Pass"))
                    {
                        testResults.Add(new KeyValuePair<int, bool>(testId, true));
                    }
                    else
                    {
                        totalFaults++;
                        testResults.Add(new KeyValuePair<int, bool>(testId, false));
                    }
                }
            }
            #endregion



            var originalOrder = (from t in testId_pair select t.Key).ToList();

            var faults = testResults.Where(x => x.Value == false);


            File.WriteAllText(fileToSaveResultForCharts, "INITIATING ....................................\n\n");
            for (var indexForRandomizingResult = 0; indexForRandomizingResult < 100; indexForRandomizingResult++)
                FindMetrics(80, indexForRandomizingResult, sortedTestId_countOfPair, testId_pair, uniquePairs, testResults, totalFaults, originalOrder, "");



            //System.Diagnostics.Process.Start(fileToSaveResultForCharts);
            ProcessStartInfo pi = new ProcessStartInfo(fileToSaveResultForCharts);
            pi.Arguments = Path.GetFileName(fileToSaveResultForCharts);
            pi.UseShellExecute = true;
            pi.WorkingDirectory = Path.GetDirectoryName(fileToSaveResultForCharts);
            pi.FileName = @"C:\Program Files (x86)\Notepad++\\notepad++.exe";
            pi.Verb = "OPEN";
            Process.Start(pi);


        }
        public void OrderForNounVerbPairExcel(bool interactive)
        {
            var results = DeserializeResults(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\LatestVersions\ExtractedObjectForAllTestResults.txt");

            var files = Directory.GetFiles(directoryForExtractedTestCases).Where(x => x.Contains("UniquePairs"));
            foreach (var file in files)
            {
                List<string> uniquePairs;
                List<KeyValuePair<int, List<string>>> testId_pair;

                ReadUniquePairsAndTestCasesWithPairs(out uniquePairs, out testId_pair, file);
                #region Counting Pair Support, Each pair with its count
                List<KeyValuePair<string, int>> pairCounts = new List<KeyValuePair<string, int>>();
                var iCount = 0.0;
                foreach (var pair in uniquePairs)
                {
                    iCount++;
                    //get count of test cases having this pair
                    var count = (from tip in testId_pair
                                 where tip.Value.Contains(pair)
                                 select tip).Count();
                    //storing each pair with its count
                    pairCounts.Add(new KeyValuePair<string, int>(pair, count));
                    Console.Clear();
                    Console.Out.WriteLine("Pair Counting: " + iCount / uniquePairs.Count * 100 + "%");
                }
                //ordering pairs with their counts
                pairCounts = pairCounts.OrderByDescending(x => x.Value).ToList();
                #endregion

                #region Preparing Data for ordering
                //ordering the list of only pairs with the help of the list of pair with counts that was sorted in the previous step
                uniquePairs = (from u in pairCounts
                               join p in uniquePairs on u.Key equals p
                               select u.Key).ToList();

                //taking counts of Pairs with each test case
                var testId_countOfPair = (from p in testId_pair
                                          select new KeyValuePair<int, int>(p.Key, p.Value.Count)).ToList();
                //ordering Test cases by counts
                var sortedTestId_countOfPair = testId_countOfPair.OrderByDescending(x => x.Value).ToList();
                #endregion

                #region Extracting Test Results from the File
                //extracting the results of the test cases of this litmus
                /******************************IMPORTANT*******************************/
                /********Go to the result file, and replace § character with ~*********/
                
                var totalFaults = 0;
                List<KeyValuePair<int, bool>> testResults = new List<KeyValuePair<int, bool>>();
                var currentFile = file.Substring(file.LastIndexOf("\\") + 1).Replace("Tagged", "").Split('~');
                var currentProduct = currentFile[0];
                var currentVersion = currentFile[1].Split('_')[0];
                foreach (var result in results.Where(x => x.Product == currentProduct && x.Version == currentVersion))
                {
                    testResults.Add(new KeyValuePair<int, bool>(result.Id, result.Result));
                }
                totalFaults = testResults.Count(x => x.Value == false);
                #endregion



                var originalOrder = (from t in testId_pair select t.Key).ToList();

                var faults = testResults.Where(x => x.Value == false);

                var fileToSaveResultsTo = file.Substring(0, file.LastIndexOf("\\") + 1) + "Results\\NAPFD\\" + file.Substring(file.LastIndexOf("\\") + 1, file.LastIndexOf("_") - 1 - file.LastIndexOf("\\")).Replace("Tagged", "") + ".txt";

                File.WriteAllText(fileToSaveResultsTo, "INITIATING ....................................\n\n");
                for (var indexForRandomizingResult = 0; indexForRandomizingResult < 100; indexForRandomizingResult++)
                    FindMetrics(80, indexForRandomizingResult, sortedTestId_countOfPair, testId_pair, uniquePairs, testResults, totalFaults, originalOrder, fileToSaveResultsTo);



                //System.Diagnostics.Process.Start(fileToSaveResultForCharts);
                //ProcessStartInfo pi = new ProcessStartInfo(fileToSaveResultsTo);
                //pi.Arguments = Path.GetFileName(fileToSaveResultsTo);
                //pi.UseShellExecute = true;
                //pi.WorkingDirectory = Path.GetDirectoryName(fileToSaveResultsTo);
                //pi.FileName = @"C:\Program Files (x86)\Notepad++\\notepad++.exe";
                //pi.Verb = "OPEN";
                //Process.Start(pi);

            }


        }

        private void FindMetrics(int litmusNumber, int indexForRandomizingResult, List<KeyValuePair<int, int>> sortedTestId_countOfPair, List<KeyValuePair<int, List<string>>> testId_pair, List<string> uniquePairs, List<KeyValuePair<int, bool>> testResults, int totalFaults, List<int> originalOrder, string fileToSave)
        {
            #region Ordering Test cases
            //Hence the first one with most pairs is my first ranked test case
            var MyRank = new List<int>();
            if (indexForRandomizingResult >= sortedTestId_countOfPair.Count)
                return;
            MyRank.Add(sortedTestId_countOfPair.ElementAt(indexForRandomizingResult).Key);
            //removing it from the list so that it doesn't get ranked again
            //no need to do this as we are already excluding the ranked ones below
            sortedTestId_countOfPair.RemoveAt(indexForRandomizingResult);

            //now I dont want to cover the pairs already covered by my test case, so subtracting them
            //subtracting all the pairs that appear with the previously ranked test case
            var uncoveredPairs = uniquePairs.Except(testId_pair.FirstOrDefault(x => x.Key == MyRank.FirstOrDefault()).Value).ToList();
            var totalPairCounts = uncoveredPairs.Count;
            //now loop on uncovered pairs
            for (var i = 0; uncoveredPairs.Any(); i++)
            {

                var pairNowCovering = uncoveredPairs.FirstOrDefault();
                //and find the testcases covering that pair, exclude the ones already ranked
                var testCaseHavingThisPairAndWithMaxTotalPairs = (from t in testId_pair
                                                                  where t.Value.Contains(pairNowCovering)
                                                                  && !MyRank.Contains(t.Key)
                                                                  select new { t.Key, t.Value.Count });
                //now take the one covering maximum number of pairs
                if (testCaseHavingThisPairAndWithMaxTotalPairs.Any())
                {
                    var testCaseWithMaxTotalPairs = testCaseHavingThisPairAndWithMaxTotalPairs.ToList().OrderByDescending(x => x.Count).FirstOrDefault().Key;
                    MyRank.Add(testCaseWithMaxTotalPairs);
                    sortedTestId_countOfPair.Remove(sortedTestId_countOfPair.FirstOrDefault(x => x.Key == testCaseWithMaxTotalPairs));
                }
                //removing the covered pair so that it doesnt get covered again
                uncoveredPairs.Remove(pairNowCovering);
                if (uncoveredPairs.Count > 0)
                    Console.Out.WriteLine("Ordering Progress: " + (float)i / totalPairCounts * 100 + "%");
            }


            #region Random Prioritization
            //FOR Random ordering
            var randomRanking = new List<int>();
            var randomNumbers = GenerateRandom(originalOrder.Count, 0, originalOrder.Count);
            List<int> alreadyUsedIndexes = new List<int>();
            foreach (var rnd in randomNumbers)
            {
                randomRanking.Add(originalOrder[rnd]);
            }
            #endregion

            //now you will be done with all the pairs but test cases still remaining, so copy them according to their order(counts of pairs included in a test case) in MyRank
            if (sortedTestId_countOfPair.Count > 0)
            {
                MyRank.AddRange((from t in sortedTestId_countOfPair select t.Key).ToList());
            }
            #endregion
            if (string.IsNullOrEmpty(fileToSave))
                File.AppendAllText(fileToSaveResultForCharts, "ITERATION: " + indexForRandomizingResult + " # \n");
            else
                File.AppendAllText(fileToSave, "ITERATION: " + indexForRandomizingResult + " # \n");
            #region Plotting Graph for Percentage of Faults found against test cases
            ///////////////////////////PERCENTAGE OF FAULTS FOUND/////////////////////////////

            //calculating apfd
            var myAPFD = 1.0;
            double[] apfdPrioritizedForIteration = new double[MyRank.Count];
            var indexDetectingFault = 0;
            var loope = 0;
            var faultsDetected = 0.0;
            Dictionary<float, float> faultDetectedPercentage = new Dictionary<float, float>();
            foreach (var rank in MyRank)
            {
                loope++;
                if (testResults.Any(x => x.Key == rank && x.Value == false))
                {
                    faultsDetected++;
                    indexDetectingFault += loope;
                    myAPFD = NAPFDConstant - ((float)indexDetectingFault / (float)(totalFaults * MyRank.Count)) + (float)(NAPFDConstant / (float)(2 * MyRank.Count));
                    faultDetectedPercentage.Add((float)loope / MyRank.Count * 100, (float)faultsDetected / totalFaults);
                }
                Console.Out.WriteLine((float)loope / MyRank.Count * 100 + "%: " + (float)faultsDetected / totalFaults);
            }

            if (string.IsNullOrEmpty(fileToSave))
            {
                File.AppendAllText(fileToSaveResultForCharts, "PAIR METHOD APFD = " + myAPFD + ";");
                File.AppendAllText(fileToSaveResultForCharts, "\t\t\t" + string.Join(",", faultDetectedPercentage) + "\n");
            }
            else
            {
                File.AppendAllText(fileToSave, "PAIR METHOD APFD = " + myAPFD + ";");
                File.AppendAllText(fileToSave, "\t\t\t" + string.Join(",", faultDetectedPercentage) + "\n");
            }

            //indexDetectingFault = 0;
            //loope = 0;
            //faultsDetected = 0.0;
            //faultDetectedPercentage = new Dictionary<float, float>();
            //foreach (var rank in originalOrder)
            //{
            //    loope++;
            //    if (testResults.Any(x => x.Key == rank && x.Value == false))
            //    {
            //        faultsDetected++;
            //        indexDetectingFault += loope;
            //        myAPFD = 1.0 - ((float)indexDetectingFault / (float)(totalFaults * originalOrder.Count)) + (float)(1.0 / (float)(2 * originalOrder.Count));
            //        faultDetectedPercentage.Add((float)loope / originalOrder.Count * 100, (float)faultsDetected / totalFaults);
            //    }
            //    Console.Out.WriteLine((float)loope / originalOrder.Count * 100 + "%: " + (float)faultsDetected / totalFaults);
            //}
            //File.AppendAllText(fileToSaveResultForCharts, "ORIGINAL ORDER APFD = " + myAPFD + "\n");

            indexDetectingFault = 0;
            loope = 0;
            faultsDetected = 0.0;
            faultDetectedPercentage = new Dictionary<float, float>();
            var randomAPFD = 0.0;
            foreach (var rank in randomRanking)
            {
                loope++;
                if (testResults.Any(x => x.Key == rank && x.Value == false))
                {
                    faultsDetected++;
                    indexDetectingFault += loope;
                    randomAPFD = NAPFDConstant - ((float)indexDetectingFault / (float)(totalFaults * randomRanking.Count)) + (float)(NAPFDConstant / (float)(2 * randomRanking.Count));
                    faultDetectedPercentage.Add((float)loope / randomRanking.Count * 100, (float)faultsDetected / totalFaults);
                }
                Console.Out.WriteLine((float)loope / randomRanking.Count * 100 + "%: " + (float)faultsDetected / totalFaults);
            }
            if (string.IsNullOrEmpty(fileToSave))
            {
                File.AppendAllText(fileToSaveResultForCharts, "RANDOM METHOD APFD = " + randomAPFD + ";");
                File.AppendAllText(fileToSaveResultForCharts, "\t\t\t" + string.Join(",", faultDetectedPercentage) + "\n");
            }
            else
            {
                File.AppendAllText(fileToSave, "RANDOM METHOD APFD = " + randomAPFD + ";");
                File.AppendAllText(fileToSave, "\t\t\t" + string.Join(",", faultDetectedPercentage) + "\n");
            }
            ///////////////////////////PERCENTAGE OF FAULTS FOUND/////////////////////////////
            #endregion



        }



        #endregion

        #region Not Required Anymore

        //NO NEED OF THIS AS PARAGRAPH TAG IS ALREADY INCLUDED IN THE FIRST FUNCTION

        public void ExtractMissingTestCases(List<string> files)
        {
            string allFiles = "";
            List<string> notFoundStepTextsIds = new List<string>();
            foreach (var file in files)
            {
                string contents = File.ReadAllText(file);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(contents);

                // Testcase Disabled
                if (doc.DocumentNode.SelectSingleNode("//head/title").InnerText.ToLower().Contains("testcase disabled"))
                {
                    continue;
                }

                var stepHtml = doc.DocumentNode.SelectSingleNode(".//div[@class='dv']"); //doc.GetElementbyId("steps_text");
                if (stepHtml != null)
                {
                    allFiles += "Test Case ID: " + file.Substring(file.LastIndexOf("id") + 3) + "~" + file + "\n";
                    string steps = "";
                    var secDoc = new HtmlDocument();
                    secDoc.LoadHtml(stepHtml.InnerHtml);
                    if (stepHtml.InnerHtml.Contains("<p>"))
                        foreach (var node in secDoc.DocumentNode.SelectNodes(".//p"))
                        {
                            steps += node.InnerText + " \n";
                        }
                    allFiles += steps + "\n \n \n";
                    Console.WriteLine(steps);
                }
                else { notFoundStepTextsIds.Add(file); }
            }
            Console.In.Read();
        }
        public void FindIfTestCasesMissing(bool interactive)
        {
            bool first = true;
            string contents = File.ReadAllText(fileToSaveExtractedAndTaggedTestCases);
            string[] lines = contents.Split(new string[] { "\n"/*Environment.NewLine*/ }, StringSplitOptions.RemoveEmptyEntries);
            int linesAfterId = 0;
            List<int> emptyCaseIds = new List<int>();
            List<string> emptyCaseAddresses = new List<string>();
            string lastLine = "";
            foreach (var line in lines)
            {
                if (first)
                {
                    first = false;
                    linesAfterId = 0;
                    lastLine = line;
                    continue;
                }
                if (line.Contains("Test Case ID: "))
                {

                    if (linesAfterId < 3)
                    {
                        emptyCaseIds.Add(Convert.ToInt32(lastLine.Split(':')[1].Split('~')[0]));
                        emptyCaseAddresses.Add(lastLine.Split('~')[1]);
                    }
                    linesAfterId = 0;
                    lastLine = line;
                }
                else
                {
                    linesAfterId++;
                }
            }
            if (interactive)
            {
                Console.Out.Write(String.Join(",", emptyCaseIds));
                Console.In.Read();
            }
            ExtractMissingTestCases(emptyCaseAddresses);
        }

        #endregion

        #region NotUsedMultipletPart
        /*
        public void MakeNounVerbMultiplets(bool interactive)
        {
            string[] verbTags = { "VV", "VB", "VBD", "VBG", "VBN", "VBP", "VBZ" };
            var contents = File.ReadAllText(fileToSaveExtractedAndTaggedTestCases);
            var finalString = new StringBuilder();
            int currentTestCaseID = -1;
            var allMultipletsOneList = new List<string>();
            var testCase_NounVerbMultiplets = new List<KeyValuePair<int, List<string>>>();
            var totalLines = contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Count();
            var passedLines = 0;

            foreach (var line in contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!line.Contains("Test Case ID: "))
                {
                    string previousTag = "";
                    string previousWord = "";
                    List<string> CurrentMultiplet = new List<string>();
                    foreach (var taggedWord in line.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Contains('/')))
                    {
                        string currentWord = taggedWord.Split('/')[0].ToLower();
                        string currentTag = taggedWord.Split('/')[1];
                        //IF this word is a verb, then store the word as previous word (to be used as the first word in a pair)
                        if (verbTags.Contains(currentTag))
                        {


                            CurrentMultiplet = CurrentMultiplet.OrderByDescending(x => x).ToList();
                            var currStr = String.Join("_", CurrentMultiplet);
                            if (!allMultipletsOneList.Contains(currStr) && !string.IsNullOrEmpty(currStr) && currStr.Split('_').Length > 1)
                            {
                                allMultipletsOneList.Add(currStr);
                                testCase_NounVerbMultiplets.FirstOrDefault(x => x.Key == currentTestCaseID).Value.Add(currStr);

                            }
                            else
                            {

                            }

                            CurrentMultiplet = new List<string>();
                            CurrentMultiplet.Add(currentWord);
                            previousWord = currentWord;
                        }
                        else
                        {
                            //ELSE it can only be a noun, so make a pair using the previous word, and add it in the list AllPairs and finalString
                            //AND also store it in the matrix (KeyValue pair of (testcase,List of pairs))
                            if (!string.IsNullOrEmpty(previousWord))
                                CurrentMultiplet.Add(currentWord);


                            //var pair = "(" + previousWord.ToLower() + "_" + currentWord.ToLower() + ")";
                            //allPairs.Add(pair);
                            //finalString.Append(pair);
                            //testCase_NounVerbPair.FirstOrDefault(x => x.Key == currentTestCaseID).Value.Add(pair);
                        }
                    }
                }
                //IF line is starting of a test case, then store the current test case information
                else
                {
                    currentTestCaseID = Convert.ToInt32(line.Split(new String[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[1].Split('~')[0]);
                    testCase_NounVerbMultiplets.Add(new KeyValuePair<int, List<string>>(currentTestCaseID, new List<string>()));
                }
                finalString.Append("\n");
                passedLines++;
                if (interactive)
                {
                    Console.Out.WriteLine((float)passedLines / (float)totalLines * 100 + "%");
                }
            }

            //Save the Multiplets and testcase IDs with pairs
            WriteUniqueMultipletsAndTestCasesWithMultiplets(allMultipletsOneList, testCase_NounVerbMultiplets);
        }
        public bool CheckIfMultipletExists(List<string> Multiplet, List<List<string>> ListToSearch)
        {
            foreach (var list in ListToSearch)
            {
                foreach (var multiplet in Multiplet)
                {
                    if (list.Contains(multiplet))
                        return true;
                }
                //if (list.Count == Multiplet.Count)
                //    if (list.Contains(Multiplet.ElementAt(0)))
                //        return true;
                //if (list.Contains(Multiplet.ElementAt(1)))
                //    return true;
                //if (list.Contains(Multiplet.ElementAt(2)))
                //    return true;
            }
            return false;
        }
        public void WriteUniqueMultipletsAndTestCasesWithMultiplets(List<string> uniquePairs, List<KeyValuePair<int, List<string>>> testId_pair)
        {
            File.WriteAllText(fileToSaveUniqueMultiplets, string.Join(",", uniquePairs));
            StringBuilder fileString = new StringBuilder();
            foreach (var test in testId_pair)
            {
                fileString.Append("id=" + test.Key + "\n");
                fileString.Append(string.Join(",", test.Value));
                fileString.Append("\n");
            }
            File.WriteAllText(fileToSaveTestIdWithUniqueMultiplets, fileString.ToString());

        }
        */
        #endregion





























        //GIVING LOW APFD THAN ORIGINAL AND RANDOM
        #region NounVerbCountCombined
        public void CountNounAndVerbCombined(bool interactive)
        {
            var contents = File.ReadAllText(fileToSaveExtractedAndTaggedTestCases);
            var totalLines = contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Count();
            var passedLines = 0;
            int currentTestCaseID = 0;
            List<KeyValuePair<int, int>> testCaseIdWithCounts = new List<KeyValuePair<int, int>>();
            List<KeyValuePair<int, List<string>>> testCaseIdWithTopics = new List<KeyValuePair<int, List<string>>>();
            var testCaseTopicsCount = 0;
            var testCaseTopics = new List<string>();
            foreach (var line in contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.Contains("Test Case ID: "))
                {
                    if (currentTestCaseID != 0)
                    {
                        testCaseIdWithCounts.Add(new KeyValuePair<int, int>(currentTestCaseID, testCaseTopicsCount));
                        testCaseIdWithTopics.Add(new KeyValuePair<int, List<string>>(currentTestCaseID, testCaseTopics));
                        testCaseTopicsCount = 0;
                        testCaseTopics = new List<string>();
                    }
                    currentTestCaseID = Convert.ToInt32(line.Split(new String[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[1].Split('~')[0].Replace(".html", "").Replace(".txt", ""));
                }
                else
                {
                    testCaseTopicsCount += line.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Contains('/')).Count();
                    testCaseTopics.AddRange(line.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Contains('/')).ToList());
                }
                if (interactive)
                {
                    Console.Clear();
                    Console.Out.WriteLine((float)passedLines++ / (float)totalLines * 100 + "%");
                }
            }
            WriteTestCasesWithTopicCounts(testCaseIdWithCounts, testCaseIdWithTopics);
        }
        public void WriteTestCasesWithTopicCounts(List<KeyValuePair<int, int>> testId_pair, List<KeyValuePair<int, List<string>>> testId_topics)
        {
            StringBuilder fileString = new StringBuilder();
            for (var test = 0; test < testId_pair.Count; test++)
            {
                fileString.Append("id=" + testId_pair[test].Key + "\n");
                fileString.Append(string.Join(",", testId_topics[test].Value).ToLower());
                fileString.Append("\n");
                fileString.Append(testId_pair[test].Value);
                fileString.Append("\n");
            }
            File.WriteAllText(fileToSaveTestIdWithTopicCounts, fileString.ToString());
        }
        public void ReadTestCasesWithTopicCounts(out List<KeyValuePair<int, int>> testId_Count, out List<KeyValuePair<int, List<string>>> testId_Topics)
        {
            testId_Count = new List<KeyValuePair<int, int>>();
            testId_Topics = new List<KeyValuePair<int, List<string>>>();
            var testIdPairsContent = File.ReadAllText(fileToSaveTestIdWithTopicCounts);
            var currentId = 0;
            foreach (var line in testIdPairsContent.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.StartsWith("id="))
                    currentId = Convert.ToInt32(line.Split('=')[1]);
                else
                {
                    int temp = 0;
                    if (int.TryParse(line, out temp))
                        testId_Count.Add(new KeyValuePair<int, int>(currentId, Convert.ToInt32(line)));
                    else
                        testId_Topics.Add(new KeyValuePair<int, List<string>>(currentId, line.Split(',').ToList()));
                }
            }
        }
        public void OrderForTopicCounts(bool interactive)
        {
            List<KeyValuePair<int, int>> testId_count = new List<KeyValuePair<int, int>>();
            List<KeyValuePair<int, List<string>>> testId_topics = new List<KeyValuePair<int, List<string>>>();
            ReadTestCasesWithTopicCounts(out testId_count, out testId_topics);

            //List<int> sortedTestIds = testId_count.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();
            List<string> topics = (from t in testId_topics select t.Value).ToList().SelectMany(l => l).Distinct().ToList();
            List<KeyValuePair<string, int>> topicCounts = new List<KeyValuePair<string, int>>();
            //Getting counts of topics
            foreach (var t in topics)
            {
                var count = (from tests in testId_topics
                             where tests.Value.Contains(t)
                             select tests.Key).Count();
                topicCounts.Add(new KeyValuePair<string, int>(t, count));
            }


            /********Go to the result file, and replace § character with ~*********/
            var resultFile = File.ReadAllLines(fileToExtractResultsOfTestCasesFrom);
            var totalFaults = 0;
            //record results in this list, and put true if a test case passes else false for fail or bad test case
            List<KeyValuePair<int, bool>> testResults = new List<KeyValuePair<int, bool>>();
            foreach (var line in resultFile)
            {
                if (line.Contains("id="))
                {
                    var testId = Convert.ToInt32(line.Substring(line.IndexOf("id=") + 3/*for i d =*/).Split('~')[0]);
                    if (line.Substring(0, 10).Contains("Pass"))
                    {
                        testResults.Add(new KeyValuePair<int, bool>(testId, true));
                    }
                    else
                    {
                        totalFaults++;
                        testResults.Add(new KeyValuePair<int, bool>(testId, false));
                    }
                }
            }

            var originalOrder = (from t in testId_count
                                 select t.Key).ToList();

















            File.WriteAllText(fileToSaveResultForChartsNounVerbCount, "INITIATING ....................................\n\n");
            for (var indexForRandomizingResult = 0; indexForRandomizingResult < 100; indexForRandomizingResult++)
                FindMetricsForNounVerbCombined(indexForRandomizingResult, testId_count, topicCounts, testId_topics, testResults, totalFaults, originalOrder, "");



            //System.Diagnostics.Process.Start(fileToSaveResultForChartsNounVerbCount);
            ProcessStartInfo pi = new ProcessStartInfo(fileToSaveResultForChartsNounVerbCount);
            pi.Arguments = Path.GetFileName(fileToSaveResultForChartsNounVerbCount);
            pi.UseShellExecute = true;
            pi.WorkingDirectory = Path.GetDirectoryName(fileToSaveResultForChartsNounVerbCount);
            pi.FileName = @"C:\Program Files (x86)\Notepad++\\notepad++.exe";
            pi.Verb = "OPEN";
            Process.Start(pi);















            
            ////CLUSTERING
            //int clusterCount = 2; var testing = Cluster(dataForClustering, clusterCount); ShowVector(testing, true); ShowClustered(dataForClustering, testing, clusterCount, 1);

            ////SHOW CLUSTERED PAIRS           //for (var c = 0; c < clusterCount; c++)            //{            //    Console.Out.WriteLine("Cluster "+ c);            //    for (var d = 0; d < testing.Count() ; d++)            //    {            //        if(testing[d] == c)            //        Console.Out.Write(PairWithIds[d]);            //    }            //    Console.Out.WriteLine("-----------------------------------------------------------------------------------------------------------------");            //    Console.Out.WriteLine("-----------------------------------------------------------------------------------------------------------------");            //    Console.Out.WriteLine("-----------------------------------------------------------------------------------------------------------------");            //    Console.Out.WriteLine("-----------------------------------------------------------------------------------------------------------------");            //    Console.Out.WriteLine("-----------------------------------------------------------------------------------------------------------------");            //}
        }

        public void FindMetricsForNounVerbCombined(int indexForIteration, List<KeyValuePair<int, int>> testId_count, List<KeyValuePair<string, int>> topicCounts, List<KeyValuePair<int, List<string>>> testId_topics, List<KeyValuePair<int, bool>> testResults, int totalFaults, List<int> originalOrder, string fileToSaveResults) {


            var faults = testResults.Where(x => x.Value == false);

            var faultCount = faults.Count(x => x.Value == false);

            var MyRank = new List<int>();
            MyRank.Add(testId_count.OrderByDescending(x => x.Value).ElementAt(indexForIteration).Key);
            //removing it from the list so that it doesn't get ranked again
            //no need to do this as we are already excluding the ranked ones below
            //testId_count.RemoveAt(index);














            
            List<string> sortedTopics = topicCounts.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();
            sortedTopics = sortedTopics.Except(testId_topics.FirstOrDefault(x => x.Key == MyRank.FirstOrDefault()).Value).ToList();
            while (sortedTopics.Any())
            {
                var thisTopic = sortedTopics.FirstOrDefault();
                var testCasesCoveringThisTopic = from t in testId_topics
                                                 where t.Value.Contains(thisTopic) && !MyRank.Contains(t.Key)
                                                 select new KeyValuePair<int, int>(t.Key, t.Value.Count);
                if (testCasesCoveringThisTopic.Any())
                {
                    var bestCandidate = testCasesCoveringThisTopic.OrderByDescending(x => x.Value).FirstOrDefault();
                    MyRank.Add(bestCandidate.Key);
                    sortedTopics = sortedTopics.Except(testId_topics.FirstOrDefault(x => x.Key == bestCandidate.Key).Value).ToList();
                }
            }
            if (MyRank.Count != testId_count.Count)
            {
                var remaining = (from t in testId_count
                                 where !MyRank.Contains(t.Key)
                                 select t).OrderByDescending(x => x.Value).Select(x => x.Key).ToList();
                MyRank.AddRange(remaining);
            }


            if (string.IsNullOrEmpty(fileToSaveResults))
                File.AppendAllText(fileToSaveResultForChartsNounVerbCount, "ITERATION: " + indexForIteration + " # \n");
            else
                File.AppendAllText(fileToSaveResults, "ITERATION: " + indexForIteration + " # \n");
            ///////////////////////////PERCENTAGE OF FAULTS FOUND/////////////////////////////
            Dictionary<float, float> faultDetectedPercentage = new Dictionary<float, float>();
            //Console.In.Read();
            //calculating apfd
            var loope = 0;
            var faultsDetected = 0.0;
            var myAPFD = 1.0;
            var progress = 0.0;
            double[] apfdPrioritizedForIteration = new double[MyRank.Count];
            var indexDetectingFault = 0;
            for (var index = 0; index < MyRank.Count; index++)
            {
                var thisTest = testResults.FirstOrDefault(x => x.Key == MyRank[index]);
                if (thisTest.Key != 0 && thisTest.Value == false)
                {
                    indexDetectingFault += index;
                    myAPFD = 1 - ((float)indexDetectingFault / (float)(totalFaults * MyRank.Count)) + (float)(1 / (float)(2 * MyRank.Count));
                }
                Console.Out.WriteLine("Progress: " + progress / MyRank.Count + "%, APFD: " + myAPFD);
                apfdPrioritizedForIteration[Convert.ToInt32(progress)] = myAPFD;
                if (faults.Any(x=>x.Key == MyRank[index]))//testResults.Any(x => x.Key == MyRank[index] && x.Value == false))
                {
                    faultsDetected++;
                    faultDetectedPercentage.Add((float)(progress / MyRank.Count), (float)faultsDetected / faultCount);
                }
                progress++;
            }

            //0.56019               for Litmus 40
            //0.69075774404759671   for Litmus 50  
            //0.76381420163048475   for Litmus 60  
            //0.63726620689453073   for Litmus 70  
            //0.53781461899560423   for Litmus 80



            if (string.IsNullOrEmpty(fileToSaveResults))
            {
                File.AppendAllText(fileToSaveResultForChartsNounVerbCount, "TOPIC METHOD APFD = " + myAPFD + ";");
                File.AppendAllText(fileToSaveResultForChartsNounVerbCount, "\t\t\t" + string.Join(",", faultDetectedPercentage) + "\n");
            }
            else
            {
                File.AppendAllText(fileToSaveResults, "TOPIC METHOD APFD = " + myAPFD + ";");
                File.AppendAllText(fileToSaveResults, "\t\t\t" + string.Join(",", faultDetectedPercentage) + "\n");
            }







            //FOR Random ordering
            var randomRanking = new List<int>();
            var randomNumbers = GenerateRandom(originalOrder.Count, 0, originalOrder.Count);
            List<int> alreadyUsedIndexes = new List<int>();
            foreach (var rnd in randomNumbers)
            {
                randomRanking.Add(originalOrder[rnd]);
            }


            var randomAPFD = 1.0;
            faultsDetected = 0;
            progress = 0.0;
            double[] apfdForIteration = new double[randomRanking.Count];
            indexDetectingFault = 0;
            faultDetectedPercentage = new Dictionary<float, float>();
            for (var index = 0; index < originalOrder.Count; index++)
            {
                var thisTest = testResults.FirstOrDefault(x => x.Key == randomRanking[index]);
                if (thisTest.Key != 0 && thisTest.Value == false)
                {
                    indexDetectingFault += index;
                    randomAPFD = 1 - ((float)indexDetectingFault / (float)(totalFaults * originalOrder.Count)) + (float)(1 / (float)(2 * originalOrder.Count));
                }
                Console.Out.WriteLine("Progress: " + progress / MyRank.Count + "%, APFD: " + randomAPFD);
                apfdForIteration[Convert.ToInt32(progress)] = randomAPFD;
                if (faults.Any(x => x.Key == randomRanking[index]))//testResults.Any(x => x.Key == MyRank[index] && x.Value == false))
                {
                    faultsDetected++;
                    faultDetectedPercentage.Add((float)(progress / MyRank.Count), (float)faultsDetected / faultCount);
                }
                progress++;
            }
            
            //Console.In.Read();







            if (string.IsNullOrEmpty(fileToSaveResults))
            {
                File.AppendAllText(fileToSaveResultForChartsNounVerbCount, "RANDOM METHOD APFD = " + randomAPFD + ";");
                File.AppendAllText(fileToSaveResultForChartsNounVerbCount, "\t\t\t" + string.Join(",", faultDetectedPercentage) + "\n");
            }
            else
            {
                File.AppendAllText(fileToSaveResults, "RANDOM METHOD APFD = " + randomAPFD + ";");
                File.AppendAllText(fileToSaveResults, "\t\t\t" + string.Join(",", faultDetectedPercentage) + "\n");
            }

        }
        #endregion









        static System.Random random = new System.Random();

        #region Custom random method for returning all random in a range at once
        public static List<int> GenerateRandom(int count, int min, int max)
        {

            //  initialize set S to empty
            //  for J := N-M + 1 to N do
            //    T := RandInt(1, J)
            //    if T is not in S then
            //      insert T in S
            //    else
            //      insert J in S
            //
            // adapted for C# which does not have an inclusive Next(..)
            // and to make it from configurable range not just 1.

            if (max <= min || count < 0 ||
                    // max - min > 0 required to avoid overflow
                    (count > max - min && max - min > 0))
            {
                // need to use 64-bit to support big ranges (negative min, positive max)
                throw new ArgumentOutOfRangeException("Range " + min + " to " + max +
                        " (" + ((Int64)max - (Int64)min) + " values), or count " + count + " is illegal");
            }

            // generate count random values.
            HashSet<int> candidates = new HashSet<int>();

            // start count values before max, and end at max
            for (int top = max - count; top < max; top++)
            {
                // May strike a duplicate.
                // Need to add +1 to make inclusive generator
                // +1 is safe even for MaxVal max value because top < max
                if (!candidates.Add(random.Next(min, top + 1)))
                {
                    // collision, add inclusive max.
                    // which could not possibly have been added before.
                    candidates.Add(top);
                }
            }

            // load them in to a list, to sort
            List<int> result = candidates.ToList();

            // shuffle the results because HashSet has messed
            // with the order, and the algorithm does not produce
            // random-ordered results (e.g. max-1 will never be the first value)
            for (int i = result.Count - 1; i > 0; i--)
            {
                int k = random.Next(i + 1);
                int tmp = result[k];
                result[k] = result[i];
                result[i] = tmp;
            }
            return result;
        }
        #endregion

        #region Clustering


        static void ShowVector(int[] vector, bool newLine)
        {
            String fileString = "";
            for (int i = 0; i < vector.Length; ++i)
            {
                Console.Write(vector[i] + " ");
                fileString += vector[i] + " ";
            }
            if (newLine)
            {
                Console.WriteLine("\n");
                fileString += "\n";
            }
            File.WriteAllText(@"C:\Temp\CLUSTERINGOUTPUT.txt", fileString);
        }

        static void ShowClustered(double[][] data, int[] clustering, int numClusters, int decimals)
        {
            string fileString = "";
            for (int k = 0; k < numClusters; ++k)
            {
                fileString += "===================" + "\n";
                Console.WriteLine("===================");
                for (int i = 0; i < data.Length; ++i)
                {
                    int clusterID = clustering[i];
                    if (clusterID != k) continue;
                    Console.Write(i.ToString().PadLeft(3) + " ");
                    fileString += i.ToString().PadLeft(3) + " ";
                    for (int j = 0; j < data[i].Length; ++j)
                    {
                        if (data[i][j] >= 0.0) Console.Write(" ");
                        Console.Write(data[i][j].ToString("F" + decimals) + " ");
                        fileString += data[i][j].ToString("F" + decimals) + " ";
                    }
                    Console.WriteLine("");
                    fileString += "\n";
                }
                Console.WriteLine("===================");
                fileString += "===================" + "\n";
            } // k

            File.AppendAllText(@"C:\Temp\CLUSTERINGOUTPUT.txt", "\n\n\n\n\n\n\n\n\n\n" + fileString);
        }

        public static int[] Cluster(double[][] rawData, int numClusters)
        {
            // k-means clustering
            // index of return is tuple ID, cell is cluster ID
            // ex: [2 1 0 0 2 2] means tuple 0 is cluster 2, tuple 1 is cluster 1, tuple 2 is cluster 0, tuple 3 is cluster 0, etc.
            // an alternative clustering DS to save space is to use the .NET BitArray class
            double[][] data = Normalized(rawData); // so large values don't dominate

            bool changed = true; // was there a change in at least one cluster assignment?
            bool success = true; // were all means able to be computed? (no zero-count clusters)

            // init clustering[] to get things started
            // an alternative is to initialize means to randomly selected tuples
            // then the processing loop is
            // loop
            //    update clustering
            //    update means
            // end loop
            int[] clustering = InitClustering(data.Length, numClusters, 0); // semi-random initialization
            double[][] means = Allocate(numClusters, data[0].Length); // small convenience

            int maxCount = data.Length * 10; // sanity check
            int ct = 0;
            while (changed == true && success == true && ct < maxCount)
            {
                ++ct; // k-means typically converges very quickly
                success = UpdateMeans(data, clustering, means); // compute new cluster means if possible. no effect if fail
                changed = UpdateClustering(data, clustering, means); // (re)assign tuples to clusters. no effect if fail
            }
            // consider adding means[][] as an out parameter - the final means could be computed
            // the final means are useful in some scenarios (e.g., discretization and RBF centroids)
            // and even though you can compute final means from final clustering, in some cases it
            // makes sense to return the means (at the expense of some method signature uglinesss)
            //
            // another alternative is to return, as an out parameter, some measure of cluster goodness
            // such as the average distance between cluster means, or the average distance between tuples in 
            // a cluster, or a weighted combination of both
            return clustering;
        }

        //FINDING DISTANCE OF SOME SORT
        private static double[][] Normalized(double[][] rawData)
        {
            // normalize raw data by computing (x - mean) / stddev
            // primary alternative is min-max:
            // v' = (v - min) / (max - min)

            // make a copy of input data
            double[][] result = new double[rawData.Length][];
            for (int i = 0; i < rawData.Length; ++i)
            {
                result[i] = new double[rawData[i].Length];
                Array.Copy(rawData[i], result[i], rawData[i].Length);
            }

            //FOR EACH COLUMN
            for (int j = 0; j < result[0].Length; ++j) // each col
            {
                //TAKE COLUMN SUM
                double colSum = 0.0;
                for (int i = 0; i < result.Length; ++i)
                    colSum += result[i][j];
                //TAKE MEAN OF THAT COLUMN
                double mean = colSum / result.Length;
                //FIND STANDARD DEVIATION Sum of Xi-Xmean^2 / N
                double sum = 0.0;
                for (int i = 0; i < result.Length; ++i)
                    sum += (result[i][j] - mean) * (result[i][j] - mean);
                double sd = sum / result.Length;
                //VALUE - MEAN / STANDARD DEVIATION
                for (int i = 0; i < result.Length; ++i)
                    result[i][j] = (result[i][j] - mean) / sd;
            }
            return result;
        }

        private static int[] InitClustering(int numTuples, int numClusters, int randomSeed)
        {
            // init clustering semi-randomly (at least one tuple in each cluster)
            // consider alternatives, especially k-means++ initialization,
            // or instead of randomly assigning each tuple to a cluster, pick
            // numClusters of the tuples as initial centroids/means then use
            // those means to assign each tuple to an initial cluster.
            System.Random random = new System.Random(randomSeed);
            int[] clustering = new int[numTuples];
            for (int i = 0; i < numClusters; ++i) // make sure each cluster has at least one tuple
                clustering[i] = i;
            for (int i = numClusters; i < clustering.Length; ++i)
                clustering[i] = random.Next(0, numClusters); // other assignments random
            return clustering;
        }

        private static double[][] Allocate(int numClusters, int numColumns)
        {
            // convenience matrix allocator for Cluster()
            double[][] result = new double[numClusters][];
            for (int k = 0; k < numClusters; ++k)
                result[k] = new double[numColumns];
            return result;
        }

        private static bool UpdateMeans(double[][] data, int[] clustering, double[][] means)
        {
            // returns false if there is a cluster that has no tuples assigned to it
            // parameter means[][] is really a ref parameter

            // check existing cluster counts
            // can omit this check if InitClustering and UpdateClustering
            // both guarantee at least one tuple in each cluster (usually true)
            int numClusters = means.Length;
            int[] clusterCounts = new int[numClusters];
            for (int i = 0; i < data.Length; ++i)
            {
                int cluster = clustering[i];
                ++clusterCounts[cluster];
            }

            for (int k = 0; k < numClusters; ++k)
                if (clusterCounts[k] == 0)
                    return false; // bad clustering. no change to means[][]

            // update, zero-out means so it can be used as scratch matrix 
            for (int k = 0; k < means.Length; ++k)
                for (int j = 0; j < means[k].Length; ++j)
                    means[k][j] = 0.0;

            for (int i = 0; i < data.Length; ++i)
            {
                int cluster = clustering[i];
                for (int j = 0; j < data[i].Length; ++j)
                    means[cluster][j] += data[i][j]; // accumulate sum
            }

            for (int k = 0; k < means.Length; ++k)
                for (int j = 0; j < means[k].Length; ++j)
                    means[k][j] /= clusterCounts[k]; // danger of div by 0
            return true;
        }

        private static bool UpdateClustering(double[][] data, int[] clustering, double[][] means)
        {
            // (re)assign each tuple to a cluster (closest mean)
            // returns false if no tuple assignments change OR
            // if the reassignment would result in a clustering where
            // one or more clusters have no tuples.

            int numClusters = means.Length;
            bool changed = false;

            int[] newClustering = new int[clustering.Length]; // proposed result
            Array.Copy(clustering, newClustering, clustering.Length);

            double[] distances = new double[numClusters]; // distances from curr tuple to each mean

            for (int i = 0; i < data.Length; ++i) // walk thru each tuple
            {
                for (int k = 0; k < numClusters; ++k)
                    distances[k] = Distance(data[i], means[k]); // compute distances from curr tuple to all k means

                int newClusterID = MinIndex(distances); // find closest mean ID
                if (newClusterID != newClustering[i])
                {
                    changed = true;
                    newClustering[i] = newClusterID; // update
                }
            }

            if (changed == false)
                return false; // no change so bail and don't update clustering[][]

            // check proposed clustering[] cluster counts
            int[] clusterCounts = new int[numClusters];
            for (int i = 0; i < data.Length; ++i)
            {
                int cluster = newClustering[i];
                ++clusterCounts[cluster];
            }

            for (int k = 0; k < numClusters; ++k)
                if (clusterCounts[k] == 0)
                    return false; // bad clustering. no change to clustering[][]

            Array.Copy(newClustering, clustering, newClustering.Length); // update
            return true; // good clustering and at least one change
        }

        private static double Distance(double[] tuple, double[] mean)
        {
            // Euclidean distance between two vectors for UpdateClustering()

            double sumSquaredDiffs = 0.0;
            for (int j = 0; j < tuple.Length; ++j)
                sumSquaredDiffs += Math.Pow((tuple[j] - mean[j]), 2);
            return Math.Sqrt(sumSquaredDiffs);
        }

        private static int MinIndex(double[] distances)
        {
            // index of smallest value in array
            // helper for UpdateClustering()
            int indexOfMin = 0;
            double smallDist = distances[0];
            for (int k = 0; k < distances.Length; ++k)
            {
                if (distances[k] < smallDist)
                {
                    smallDist = distances[k];
                    indexOfMin = k;
                }
            }
            return indexOfMin;
        }
        #endregion

    }
}
