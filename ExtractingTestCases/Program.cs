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
using System.Text.RegularExpressions;

namespace ExtractingTestCases
{
    public class MainTestCase
    {
        public int TestID { get; set; }
        public int TestStepID { get; set; }
        public bool Result { get; set; }
        public MainTestCase(int testId, int stepId)
        {
            TestID = testId;
            TestStepID = stepId;
        }
        public MainTestCase(int testId, int stepId, bool result)
        {
            TestID = testId;
            TestStepID = stepId;
            Result = result;
        }
    }
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
        const string genericFolderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_@@\litmus.mozilla.org";
        const string genericFileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_@@\TestResults.txt";

        //////LITMUS 10
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_10\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_10_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_10_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_10\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_10_UniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_10_UniquePair.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_10_TestIdWithUniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_10_TestIdWithUniquePair.txt";
        //const string fileToSaveFailedTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_10_TestIdWithUniquePair_Failed.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_TestIdWithUniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_10_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_10_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_10_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\OldVersionResults\NAPFD\litmus_10_ResultForCharts_APFD_.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_10_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_10_ResultForCharts_APFD_.txt";



        ////LITMUS 11
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_11\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_11_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_11_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_11\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_11_UniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_11_UniquePair.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_11_TestIdWithUniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_11_TestIdWithUniquePair.txt";
        //const string fileToSaveFailedTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_11_TestIdWithUniquePair_Failed.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_TestIdWithUniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_11_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_11_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_11_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\OldVersionResults\APFD\litmus_11_ResultForCharts_APFD_.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_11_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_11_ResultForCharts_APFD_.txt";
        ////LITMUS 12
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_12\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_12_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_12_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_12\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_12_UniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_12_UniquePair.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_12_TestIdWithUniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_12_TestIdWithUniquePair.txt";
        //const string fileToSaveFailedTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_12_TestIdWithUniquePair_Failed.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_TestIdWithUniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_12_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_12_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_12_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\OldVersionResults\APFD\litmus_12_ResultForCharts_APFD_.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_12_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_12_ResultForCharts_APFD_.txt";

        ////LITMUS 13
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_13\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_13_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_13_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_13\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_13_UniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_13_UniquePair.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_13_TestIdWithUniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_13_TestIdWithUniquePair.txt";
        //const string fileToSaveFailedTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_13_TestIdWithUniquePair_Failed.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_TestIdWithUniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_13_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_13_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_13_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\OldVersionResults\NAPFD\litmus_13_ResultForCharts_APFD_.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_13_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_13_ResultForCharts_APFD_.txt";

        ////LITMUS 14
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_14\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_14_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_14_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_14\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_14_UniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_14_UniquePair.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_14_TestIdWithUniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_14_TestIdWithUniquePair.txt";
        //const string fileToSaveFailedTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_14_TestIdWithUniquePair_Failed.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_TestIdWithUniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_14_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_14_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_14_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\OldVersionResults\APFD\litmus_14_ResultForCharts_APFD_.txt";// @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_14_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_14_ResultForCharts_APFD_.txt";











        ////LITMUS 30
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_30\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_30_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_30_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_30\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_30_UniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_30_UniquePair.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_30_TestIdWithUniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_30_TestIdWithUniquePair.txt";
        //const string fileToSaveFailedTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_30_TestIdWithUniquePair_Failed.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_TestIdWithUniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_30_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_30_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_30_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\OldVersionResults\APFD\litmus_30_ResultForCharts_APFD_.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_30_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_30_ResultForCharts_APFD_.txt";


        ////LITMUS 35
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_35\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_35_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_35_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_35\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_35_UniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_35_UniquePair.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_35_TestIdWithUniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_35_TestIdWithUniquePair.txt";
        //const string fileToSaveFailedTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_35_TestIdWithUniquePair_Failed.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_TestIdWithUniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_35_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_35_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_35_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\OldVersionResults\APFD\litmus_35_ResultForCharts_APFD_.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_35_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_35_ResultForCharts_APFD_.txt";


        ////LITMUS 36
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_36\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_36_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_36_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_36\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_36_UniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_36_UniquePair.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_36_TestIdWithUniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_36_TestIdWithUniquePair.txt";
        //const string fileToSaveFailedTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_36_TestIdWithUniquePair_Failed.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_TestIdWithUniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_36_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_36_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_36_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\OldVersionResults\APFD\litmus_36_ResultForCharts_APFD_.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_36_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_36_ResultForCharts_APFD_.txt";













        ////LITMUS 40
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_40\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_40_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_40_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_40\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_40_UniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_40_UniquePair.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_40_TestIdWithUniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_40_TestIdWithUniquePair.txt";
        //const string fileToSaveFailedTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_40_TestIdWithUniquePair_Failed.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_TestIdWithUniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_40_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_40_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_40_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\OldVersionResults\APFD\litmus_40_ResultForCharts_APFD_.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_40_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_40_ResultForCharts_APFD_.txt";
        ////LITMUS 50
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_50\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_50\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_50_UniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_UniquePair.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_50_TestIdWithUniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_TestIdWithUniquePair.txt";
        //const string fileToSaveFailedTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_50_TestIdWithUniquePair_Failed.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_TestIdWithUniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_50_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\OldVersionResults\APFD\litmus_50_ResultForCharts_APFD_.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_50_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_50_ResultForCharts_APFD_.txt";
        //LITMUS 60
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_60\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_60_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_60_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_60\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_60_UniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_60_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_60_TestIdWithUniquePair.txt";// @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_60_TestIdWithUniquePair.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_60_TestIdWithUniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_60_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveFailedTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_60_TestIdWithUniquePair_Failed.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_TestIdWithUniquePair.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_60_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\OldVersionResults\APFD\litmus_60_ResultForCharts_APFD_.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_60_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_60_ResultForCharts_APFD_.txt";
        ////LITMUS 70
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_70\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_70_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_70_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_70\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_70_UniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_70_UniquePair.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_70_TestIdWithUniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_70_TestIdWithUniquePair.txt";
        //const string fileToSaveFailedTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_70_TestIdWithUniquePair_Failed.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_TestIdWithUniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_70_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_70_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_70_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\OldVersionResults\APFD\litmus_70_ResultForCharts_APFD_.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_70_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_70_ResultForCharts_APFD_.txt";
        //LITMUS 80
        //const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_80\litmus.mozilla.org";
        //const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_80_Rapid.txt";
        //const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_80_Rapid-Tagged.txt";
        //const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        //const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_80\TestResults.txt";
        //const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_80_UniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_80_UniquePair.txt";
        //const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_80_TestIdWithUniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_80_TestIdWithUniquePair.txt";
        //const string fileToSaveFailedTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_80_TestIdWithUniquePair_Failed.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_TestIdWithUniquePair.txt";
        //const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_80_UniqueMultiplets.txt";
        //const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_80_TestIdWithUniqueMultiplets.txt";
        //const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_80_TestIdWithTopicCounts.txt";
        //const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\OldVersionResults\APFD\litmus_80_ResultForCharts_APFD_.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_80_ResultForCharts_APFD_.txt";
        //const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_80_ResultForCharts_APFD_.txt";
        //LITMUS 90
        const string folderToLookForTestCases = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_90\litmus.mozilla.org";
        const string fileToSaveExtractedRawTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_90_Rapid.txt";
        const string fileToSaveExtractedAndTaggedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_90_Rapid-Tagged.txt";
        const string taggerModelPath = @"C:\Temp\SEALab\NLP-Project\TaggerModel\english-caseless-left3words-distsim.tagger";
        const string fileToExtractResultsOfTestCasesFrom = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_90\TestResults.txt";
        const string fileToSaveUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_90_UniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_90_UniquePair.txt";
        const string fileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_90_TestIdWithUniquePair.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_90_TestIdWithUniquePair.txt";
        const string fileToSaveFailedTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\litmus_90_TestIdWithUniquePair_Failed.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_50_TestIdWithUniquePair.txt";
        const string fileToSaveUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_90_UniqueMultiplets.txt";
        const string fileToSaveTestIdWithUniqueMultiplets = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_90_TestIdWithUniqueMultiplets.txt";
        const string fileToSaveTestIdWithTopicCounts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\NounVerbCount\litmus_90_TestIdWithTopicCounts.txt";
        const string fileToSaveResultForCharts = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\ExtractedFiles\OldVersionResults\NAPFD\litmus_90_ResultForCharts_APFD_.txt";//@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\NAPFD\litmus_90_ResultForCharts_APFD_.txt";
        const string fileToSaveResultForChartsNounVerbCount = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\OldVersionResults\CountMethod\APFD\litmus_90_ResultForCharts_APFD_.txt";

        #endregion

        const string excelPathForTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\TestSteps.xlsx";
        const string excelPathForTestCasesResults = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\TestResults.xlsx";
        const string directoryForExtractedTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\Phase 2\NewVersions";

        public static double NAPFDConstant = 1;
        static void Main(string[] args)
        {
            var program = new Program();

            //program.ExtractTestCases();
            //program.CompileResultsOldVersion();
            //program.FetchExtractedTestIDsGetWholeResultsOldVersion();
            //program.ProcessAllNounsInALitmus();
            //program.IndexAllNounsInLitmus();
            
            /*New Versions extracted from mySQL dump as CSV files*/
            //program.GetTestCasesFromExtractedCSVs();
            //program.CompileResultsMySQL();
            //program.FetchExtractedTestIDsGetWholeResultsMySQL();
            //program.MakeNounVerbPairsForMySQL("mobile");
            //program.MakeNounVerbPairsForMySQL("tablet");
            //program.OrderForNounVerbPairs("mobile");
            //program.OrderForNounVerbPairs("tablet");
            //program.MakeNounsVerbsCombinedForMySQL("mobile");
            //program.MakeNounsVerbsCombinedForMySQL("tablet");
            //program.MakeNounsOrVerbsForMySQL("mobile", "Nouns");
            //program.MakeNounsOrVerbsForMySQL("mobile", "Verbs");
            //program.MakeNounsOrVerbsForMySQL("tablet", "Nouns");
            //program.MakeNounsOrVerbsForMySQL("tablet", "Verbs");

            //program.OrderForNounsVerbs("tablet", "noun", "Frequency");
            //program.OrderForNounsVerbs("tablet", "noun", "AddGreedy");
            //program.OrderForNounsVerbs("tablet", "verb", "Frequency");
            //program.OrderForNounsVerbs("tablet", "verb", "AddGreedy");
            //program.OrderForNounsVerbs("mobile", "noun", "Frequency");
            //program.OrderForNounsVerbs("mobile", "noun", "AddGreedy");
            //program.OrderForNounsVerbs("mobile", "verb", "Frequency");
            //program.OrderForNounsVerbs("mobile", "verb", "AddGreedy");
            program.OrderForNounsVerbs("tablet", "", "Frequency");
            program.OrderForNounsVerbs("tablet", "", "AddGreedy");
            program.OrderForNounsVerbs("mobile", "", "Frequency");
            program.OrderForNounsVerbs("mobile", "", "AddGreedy");
            program.OrderForNounVerbPairs("tablet", "Frequency");
            program.OrderForNounVerbPairs("tablet", "AddGreedy");
            program.OrderForNounVerbPairs("mobile", "Frequency");
            program.OrderForNounVerbPairs("mobile", "AddGreedy");


            //program.ProcessAllNounsOrVerbsOrNounsVerbsInALitmus("mobile", "NounsOnly");
            //program.ProcessAllNounsOrVerbsOrNounsVerbsInALitmus("mobile", "VerbsOnly");
            //program.ProcessAllNounsOrVerbsOrNounsVerbsInALitmus("mobile", "NounsVerbs");
            //program.ProcessAllNounsOrVerbsOrNounsVerbsInALitmus("tablet", "NounsOnly");
            //program.ProcessAllNounsOrVerbsOrNounsVerbsInALitmus("tablet", "VerbsOnly");
            //program.ProcessAllNounsOrVerbsOrNounsVerbsInALitmus("tablet", "NounsVerbs");


            //program.IndexAllNounsOrVerbsOrNounsVerbsInLitmus("mobile", "NounsOnly");
            //program.IndexAllNounsOrVerbsOrNounsVerbsInLitmus("mobile", "VerbsOnly");
            //program.IndexAllNounsOrVerbsOrNounsVerbsInLitmus("mobile", "NounsVerbs");
            //program.IndexAllNounsOrVerbsOrNounsVerbsInLitmus("tablet", "NounsOnly");
            //program.IndexAllNounsOrVerbsOrNounsVerbsInLitmus("tablet", "VerbsOnly");
            //program.IndexAllNounsOrVerbsOrNounsVerbsInLitmus("tablet", "NounsVerbs");



            //program.OrderForStringDistanceNounVerbPairs("mobile", "NounsOnly", "E");
            //program.OrderForStringDistanceNounVerbPairs("mobile", "NounsOnly", "M");
            //program.OrderForStringDistanceNounVerbPairs("mobile", "NounsOnly", "H");
            //program.OrderForStringDistanceNounVerbPairs("mobile", "NounsOnly", "C");
            //program.OrderForStringDistanceNounVerbPairs("mobile", "VerbsOnly", "E");
            //program.OrderForStringDistanceNounVerbPairs("mobile", "VerbsOnly", "M");
            //program.OrderForStringDistanceNounVerbPairs("mobile", "VerbsOnly", "C");
            //program.OrderForStringDistanceNounVerbPairs("mobile", "VerbsOnly", "H");
            //program.OrderForStringDistanceNounVerbPairs("tablet", "NounsOnly", "E");
            //program.OrderForStringDistanceNounVerbPairs("tablet", "NounsOnly", "M");
            //program.OrderForStringDistanceNounVerbPairs("tablet", "NounsOnly", "H");
            //program.OrderForStringDistanceNounVerbPairs("tablet", "NounsOnly", "C");
            //program.OrderForStringDistanceNounVerbPairs("tablet", "VerbsOnly", "E");
            //program.OrderForStringDistanceNounVerbPairs("tablet", "VerbsOnly", "M");
            //program.OrderForStringDistanceNounVerbPairs("tablet", "VerbsOnly", "C");
            //program.OrderForStringDistanceNounVerbPairs("tablet", "VerbsOnly", "H");


            //Tablets
            //program.GetTestCasesFromExtractedCombinedCSV();
            //program.CompileResultsCombinedCSVMySQL();


            //program.ProcessAllPairsInALitmus("tablet");
            //program.IndexAllPairsInLitmus("tablet");
            //program.ProcessAllPairsInALitmus("mobile");
            //program.IndexAllPairsInLitmus("mobile");
            //program.ProcessAllNounsVerbsInALitmus("tablet");
            //program.IndexAllNounsVerbsInLitmus("tablet");
            //program.ProcessAllNounsVerbsInALitmus("mobile");
            //program.IndexAllNounsVerbsInLitmus("mobile");




            //program.MakeIndicesBinary("mobile");
            //program.AnalyzeResultFiles();
            //program.AnalyzeTestCaseFiles();
            //program.CreateResultFilesByVersion();




            #region Old
            //program.ProcessTrace();

            //program.MakeNounVerbPairs();
            //program.MakeNounOnly();
            //for (NAPFDConstant = 0.10; NAPFDConstant < 1.0; NAPFDConstant += 0.1)
            //program.OrderForNounVerbPair(); //FindIfTestCasesMissing();

            //program.MakeNounVerbMultiplets();    


            //program.CountNounAndVerbCombined();
            //program.OrderForTopicCounts();


            //program.CountNounAndVerbCombinedForExcel();
            //program.OrderForTopicCountsForExcel();

            //System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"c:\bing.wav");
            //player.Play();
            #endregion
        }


        #region Old Versions Litmus
        public void ExtractTestCases()
        {

            // Loading POS Tagger
            var tagger = new MaxentTagger(taggerModelPath);
            StringBuilder allFiles = new StringBuilder();
            StringBuilder allTaggedFiles = new StringBuilder();
            var files = Directory.EnumerateFiles(folderToLookForTestCases, "show_test.*");
            var filesProcessed = 0.0;
            var fileCount = files.Count();
            foreach (string file in files)
            {
                Console.Out.WriteLine((float)filesProcessed++ / (float)fileCount * 100 + " % ");
                //OLD:Fetching only Page 0 if multiple pages of a test case exist, same steps in all
                //NEW:Fetching Single_Result files only, because we only have results for those
                //if (file.Contains("single_result") && file.Contains("id=") && !file.Contains("&page="))
                if (file.Contains("show_test") && file.Contains("id=") && !file.Contains("&page="))
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

                    Console.WriteLine(steps);
                    Console.WriteLine(taggedSteps);
                }
            }
            //Saving test cases
            File.WriteAllText(fileToSaveExtractedRawTestCases, allFiles.ToString());
            File.WriteAllText(fileToSaveExtractedAndTaggedTestCases, allTaggedFiles.ToString());
        }
        #region Compile Result Old Version
        public void CompileResultsOldVersion()
        {
            var versions = new int[] { 10, 11, 12, 13, 30, 35, 36, 40, 50, 60, 70, 80, 90 };
            foreach (var version in versions)
            {
                var files = Directory.EnumerateFiles(genericFolderToLookForTestCases.Replace("@@", version.ToString()), "single_result.*");
                var filesProcessed = 0.0;
                var fileCount = files.Count();
                List<MainTestCase> ShowTest_SingleResultPairs = new List<MainTestCase>();
                foreach (string file in files)
                {
                    Console.Out.WriteLine((float)filesProcessed++ / (float)fileCount * 100 + " % ");
                    if (file.Contains("single_result") && file.Contains("id=") && !file.Contains("&page="))
                    {
                        string contents = File.ReadAllText(file);
                        if (contents.IndexOf("show_test.cgi?id=") != -1)
                        {
                            string testId = contents.Substring(contents.IndexOf("show_test.cgi?id=") + 17).Split('\"')[0];
                            ShowTest_SingleResultPairs.Add(new MainTestCase(Convert.ToInt32(testId), Convert.ToInt32(file.Substring(file.IndexOf("id=") + 3).Replace(".txt", "").Replace(".html", ""))));
                        }
                    }
                }


                var resultFile = File.ReadAllLines(genericFileToExtractResultsOfTestCasesFrom.Replace("@@", version.ToString()));
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
                var newObj = new List<MainTestCase>();
                foreach (var test in ShowTest_SingleResultPairs)
                {
                    if (testResults.Any(x => x.Key == test.TestStepID))
                    {
                        var result = testResults.FirstOrDefault(x => x.Key == test.TestStepID);
                        newObj.Add(new MainTestCase(test.TestID, test.TestStepID, result.Value));
                    }
                }
                Dictionary<int, bool> testResult = new Dictionary<int, bool>();
                foreach (var testStep in newObj)
                {
                    if (!testResult.Any(x => x.Key == testStep.TestID))
                    {
                        testResult.Add(testStep.TestID, testStep.Result);
                    }
                    else
                    {
                        var oldResult = testResult.FirstOrDefault(x => x.Key == testStep.TestID).Value;
                        testResult[testStep.TestID] = oldResult & testStep.Result;
                    }
                }
                File.WriteAllLines(@"C:\TEMP\Result_" + version + ".txt",
    testResult.Select(x => "[" + x.Key + " " + x.Value + "]").ToArray());

            }
            //Saving test cases
            //File.WriteAllText(fileToSaveExtractedRawTestCases, allFiles.ToString());
            //File.WriteAllText(fileToSaveExtractedAndTaggedTestCases, allTaggedFiles.ToString());
        }
        #endregion
        #region Report Statistics in C:\Temp\Statistics.txt
        public void FetchExtractedTestIDsGetWholeResultsOldVersion()
        {
            var versions = new int[] { 10, 11, 12, 13, 30, 35, 36, 40, 50, 60, 70, 80, 90 };
            foreach (var v in versions)
            {
                var tests = File.ReadAllLines(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_@@_Rapid-Tagged.txt".Replace("@@", v.ToString()));
                var results = File.ReadAllLines(@"C:\Temp\Result_@@.txt".Replace(@"@@", v.ToString()));
                var r = new Dictionary<int, bool>();
                List<int> t = new List<int>();
                var compiledResults = new Dictionary<int, bool>();
                foreach (var line in tests.Where(x => x.Contains("Test Case ID: ")))
                {
                    t.Add(Convert.ToInt32(line.Substring(line.IndexOf("id=") + 3)));
                }
                foreach (var line in results)
                {
                    var temp = line.Replace("[", "").Replace("]", "").Split(' ');
                    r.Add(Convert.ToInt32(temp[0]), Convert.ToBoolean(temp[1]));
                }
                foreach (var test in t)
                {
                    if (r.Any(x => x.Key == test))
                    {
                        var temp = r.Where(x => x.Key == test);
                        compiledResults.Add(temp.FirstOrDefault().Key, temp.FirstOrDefault().Value);
                    }
                    else
                        compiledResults.Add(test, true);
                }
                var testCaseCount = compiledResults.Count();
                var falseCount = compiledResults.Count(x => x.Value == false);
                File.AppendAllLines(@"C:\Temp\Statistics.txt", new string[] { "Version:" + v + ", \tTotal Test Cases:" + testCaseCount + "\t, Failed Test Cases:" + falseCount });
            }
        }
        #endregion
        #region Prepare Noun Verb Pairs and Noun Only for all the test cases
        public void MakeNounVerbPairs()
        {
            var versions = new int[] { 10, 11, 12, 13, 30, 35, 36, 40, 50, 60, 70, 80, 90 };
            var genericFileNameToAbstract = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_@@_Rapid-Tagged.txt";
            foreach (var v in versions)
            {
                string[] verbTags = { "VV", "VB", "VBD", "VBG", "VBN", "VBP", "VBZ" };
                var contents = File.ReadAllText(genericFileNameToAbstract.Replace("@@", v.ToString()));
                var finalString = new StringBuilder();
                int currentTestCaseID = -1;
                var allPairs = new List<string>();
                var testCase_NounVerbPair = new List<KeyValuePair<int, List<string>>>();
                var totalLines = contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Count();
                var passedLines = 0;
                //Traversing through lines in tagged file
                var stepLevel = 0;
                foreach (var line in contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var currentTestCaseHeader = "";
                    //IF line is not starting of a test case
                    if (!line.Contains("Test Case ID: "))
                    {
                        //stepLevel++;
                        string previousTag = "";
                        string previousWord = "";
                        bool firstWord = true;
                        foreach (var taggedWord in line.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Contains('/')))
                        {
                            string currentWord = taggedWord.Split('/')[0];
                            string currentTag = taggedWord.Split('/')[1];
                            //IF this word is a verb, then store the word as previous word (to be used as the first word in a pair)
                            if (verbTags.Contains(currentTag) || firstWord)
                            {
                                previousTag = currentTag;
                                previousWord = currentWord;
                                firstWord = false;
                            }
                            else
                            {
                                //ELSE it can only be a noun, so make a pair using the previous word, and add it in the list AllPairs and finalString
                                //AND also store it in the matrix (KeyValue pair of (testcase,List of pairs))
                                if (!string.IsNullOrEmpty(previousWord))
                                {
                                    var pair = "" + previousWord.ToLower() + "," + currentWord.ToLower() /*+ stepLevel*/;
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
                        stepLevel = 0;
                    }
                    finalString.Append("\n");
                    passedLines++;
                    Console.Clear();
                    Console.Out.WriteLine((float)passedLines / (float)totalLines * 100 + "%");
                }
                List<string> distinctPairs = new List<string>();
                distinctPairs.AddRange(allPairs.Distinct());
                //Save the pairs and testcase IDs with pairs
                WriteUniquePairsAndTestCasesWithPairs(distinctPairs, testCase_NounVerbPair, null, v.ToString());
            }
        }
        public void WriteUniquePairsAndTestCasesWithPairs(List<string> uniquePairs, List<KeyValuePair<int, List<string>>> testId_pair, string fileName, string version)
        {
            //var genericFileName = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_@@\TestResults.txt".Replace("@@", version);
            var genericFileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_@@_TestIdWithUniquePair.txt".Replace("@@", version);
            var tests = File.ReadAllLines(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_@@_Rapid-Tagged.txt".Replace("@@", version.ToString()));
            var results = File.ReadAllLines(@"C:\Temp\Result_@@.txt".Replace(@"@@", version.ToString()));
            var r = new Dictionary<int, bool>();
            List<int> t = new List<int>();
            var compiledResults = new Dictionary<int, bool>();
            foreach (var line in results)
            {
                var temp = line.Replace("[", "").Replace("]", "").Split(' ');
                r.Add(Convert.ToInt32(temp[0]), Convert.ToBoolean(temp[1]));
            }
            foreach (var line in tests.Where(x => x.Contains("Test Case ID: ")))
            {
                t.Add(Convert.ToInt32(line.Substring(line.IndexOf("id=") + 3)));
            }
            foreach (var test in t)
            {
                if (r.Any(x => x.Key == test))
                {
                    var temp = r.Where(x => x.Key == test);
                    compiledResults.Add(temp.FirstOrDefault().Key, temp.FirstOrDefault().Value);
                }
                else
                    compiledResults.Add(test, true);
            }
            StringBuilder passedFileString = new StringBuilder();
            foreach (var test in testId_pair)
            {
                var result = compiledResults.FirstOrDefault(x => x.Key == test.Key).Value;

                passedFileString.Append(test.Key + "=" + (compiledResults.FirstOrDefault(x => x.Key == test.Key).Value ? "Passed" : "Failed"));
                passedFileString.Append(Environment.NewLine);
                passedFileString.Append(string.Join(Environment.NewLine, test.Value));
                passedFileString.Append(Environment.NewLine);
                passedFileString.Append(Environment.NewLine);
            }
            File.WriteAllText(genericFileToSaveTestIdWithUniquePairs, passedFileString.ToString());
        }
        public void MakeNounOnly()
        {
            var versions = new int[] { 10, 11, 12, 13, 30, 35, 36, 40, 50, 60, 70, 80, 90 };
            var genericFileNameToAbstract = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_@@_Rapid-Tagged.txt";
            foreach (var v in versions)
            {
                string[] allowedTags = { "NN", "NNS", "NNP", "NNPS" };
                //string[] verbTags = { "VV", "VB", "VBD", "VBG", "VBN", "VBP", "VBZ" };
                var contents = File.ReadAllText(genericFileNameToAbstract.Replace("@@", v.ToString()));
                var finalString = new StringBuilder();
                int currentTestCaseID = -1;
                var allNouns = new List<string>();
                var testCase_NounOnly = new List<KeyValuePair<int, List<string>>>();
                var totalLines = contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Count();
                var passedLines = 0;
                //Traversing through lines in tagged file
                var stepLevel = 0;
                foreach (var line in contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var currentTestCaseHeader = "";
                    //IF line is not starting of a test case
                    if (!line.Contains("Test Case ID: "))
                    {
                        //stepLevel++;
                        string previousTag = "";
                        string previousWord = "";
                        bool firstWord = true;
                        foreach (var taggedWord in line.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Contains('/')))
                        {
                            string currentWord = taggedWord.Split('/')[0];
                            string currentTag = taggedWord.Split('/')[1];
                            //IF this word is a verb, then store the word as previous word (to be used as the first word in a pair)
                            if (allowedTags.Contains(currentTag) || firstWord)
                            {
                                previousTag = currentTag;
                                previousWord = currentWord;
                                firstWord = false;
                                allNouns.Add(currentWord);
                                finalString.Append(currentWord);
                                testCase_NounOnly.FirstOrDefault(x => x.Key == currentTestCaseID).Value.Add(currentWord);
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
                        testCase_NounOnly.Add(new KeyValuePair<int, List<string>>(currentTestCaseID, new List<string>()));
                        currentTestCaseHeader = line;
                        finalString.Append(currentTestCaseHeader + "\n");
                        stepLevel = 0;
                    }
                    finalString.Append("\n");
                    passedLines++;
                    Console.Clear();
                    Console.Out.WriteLine((float)passedLines / (float)totalLines * 100 + "%");
                }
                List<string> distinctPairs = new List<string>();
                distinctPairs.AddRange(allNouns.Distinct());
                //Save the pairs and testcase IDs with pairs
                WriteUniquePairsAndTestCasesWithNounsOnly(distinctPairs, testCase_NounOnly, null, v.ToString());
            }
        }
        public void WriteUniquePairsAndTestCasesWithNounsOnly(List<string> uniqueNouns, List<KeyValuePair<int, List<string>>> testId_nouns, string fileName, string version)
        {
            var genericFileName = @"C:\Temp\SEALab\NLP-Project\Litmuss\litmus_@@\TestResults.txt".Replace("@@", version);
            var genericFileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_@@_TestIdWithNouns.txt".Replace("@@", version);
            var tests = File.ReadAllLines(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\litmus_@@_Rapid-Tagged.txt".Replace("@@", version.ToString()));
            var results = File.ReadAllLines(@"C:\Temp\Result_@@.txt".Replace(@"@@", version.ToString()));
            var r = new Dictionary<int, bool>();
            List<int> t = new List<int>();
            var compiledResults = new Dictionary<int, bool>();
            foreach (var line in results)
            {
                var temp = line.Replace("[", "").Replace("]", "").Split(' ');
                r.Add(Convert.ToInt32(temp[0]), Convert.ToBoolean(temp[1]));
            }
            foreach (var line in tests.Where(x => x.Contains("Test Case ID: ")))
            {
                t.Add(Convert.ToInt32(line.Substring(line.IndexOf("id=") + 3)));
            }
            foreach (var test in t)
            {
                if (r.Any(x => x.Key == test))
                {
                    var temp = r.Where(x => x.Key == test);
                    compiledResults.Add(temp.FirstOrDefault().Key, temp.FirstOrDefault().Value);
                }
                else
                    compiledResults.Add(test, true);
            }
            StringBuilder passedFileString = new StringBuilder();
            foreach (var test in testId_nouns)
            {
                var result = compiledResults.FirstOrDefault(x => x.Key == test.Key).Value;

                passedFileString.Append(test.Key + "=" + (compiledResults.FirstOrDefault(x => x.Key == test.Key).Value ? "Passed" : "Failed"));
                passedFileString.Append(Environment.NewLine);
                passedFileString.Append(string.Join(Environment.NewLine, test.Value));
                passedFileString.Append(Environment.NewLine);
                passedFileString.Append(Environment.NewLine);
            }
            File.WriteAllText(genericFileToSaveTestIdWithUniquePairs, passedFileString.ToString());
        }

        #endregion
        #endregion

        #region MySQL New Versions

        /// <summary>
        /// STEP 1: TAGGING Test Cases, Keeping only Nouns and Verbs
        /// </summary>
        /// <param name="product"></param>
        #region Tag test suites
        public void GetTestCasesFromExtractedCombinedCSV(string product)
        {
            var versions = new string[14];
            if (product == "Tablet")
                versions = new string[] { "16 for Tablets", "17 Tablets", "18 Tablets", "19 Tablets", "20 Tablets", "21 Tablet", "22 Tablet", "23 Tablet", "24 Tablet", "25 Tablet", "26 Tablet", "27 Tablet", "28 Tablet", "29 Tablet" };
            else
                versions = new string[] { "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29" };
            var directoryToSaveTestCases = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\";
            var tagger = new MaxentTagger(taggerModelPath);
            var file = @"C:\Temp\SEALab\NLP-Project\MySqlTests-NewVersions\" + (product == "Tablet" ? "tablets" : "mobile") + "_steps_view.csv";
            Dictionary<string, List<string>> caseIdSteps = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> caseIdTaggedSteps = new Dictionary<string, List<string>>();
            var steps = new StringBuilder();
            var taggedSteps = new StringBuilder();
            var content = File.ReadAllLines(file);
            var lastId = "";
            var lineCount = 0.0;
            var testCaseId = "";
            foreach (var line in content)
            {
                if (string.IsNullOrEmpty(line))
                    continue;
                var l = line;
                var temp = l.Split(new string[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries);
                int b56 = 0;
                if (temp.Length > 2 && int.TryParse(temp[1], out b56) && lastId != temp[1])
                {
                    testCaseId = temp[0] + ":" + temp[1];
                    //Removing urls
                    var instruction = Regex.Replace(temp[2], @"http[^\s]+", "");
                    //Removing special char
                    instruction = StripSpecialCharacters(instruction);
                    //Splitting words based on "-" and "_"
                    instruction = instruction.Replace("_", " ").Replace("-", " ");
                    //Splitting camel case
                    instruction = Regex.Replace(instruction, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1");
                    if (caseIdSteps.Any(x => x.Key == testCaseId))
                    {
                        var instructions = caseIdSteps[testCaseId];
                        instructions.Add(instruction);
                        caseIdSteps[testCaseId] = instructions;

                        var sentences = MaxentTagger.tokenizeText(new java.io.StringReader(instruction)).toArray();
                        foreach (ArrayList sentence in sentences)
                        {
                            var tagged = tagger.tagSentence(sentence).ToString();
                            var taggedInstructions = caseIdTaggedSteps[testCaseId];
                            taggedInstructions.Add(FetchNounAndVerbsOnly(tagged.Substring(1, tagged.Length - 2)));
                            caseIdTaggedSteps[testCaseId] = taggedInstructions;
                        }
                    }
                    else
                    {
                        caseIdSteps.Add(testCaseId, new List<string>() { instruction });
                        caseIdTaggedSteps.Add(testCaseId, new List<string>() { });
                        var sentences = MaxentTagger.tokenizeText(new java.io.StringReader(instruction)).toArray();
                        foreach (ArrayList sentence in sentences)
                        {
                            var tagged = tagger.tagSentence(sentence).ToString();
                            var taggedInstructions = caseIdTaggedSteps[testCaseId];
                            taggedInstructions.Add(FetchNounAndVerbsOnly(tagged.Substring(1, tagged.Length - 2)));
                            caseIdTaggedSteps[testCaseId] = taggedInstructions;
                        }
                    }
                }
                else if (temp.Length == 1)
                {
                    var instruction = StripSpecialCharacters(temp[0]);
                    var instructions = caseIdSteps[testCaseId];
                    instructions.Add(instruction);
                    caseIdSteps[testCaseId] = instructions;

                    var sentences = MaxentTagger.tokenizeText(new java.io.StringReader(instruction)).toArray();
                    foreach (ArrayList sentence in sentences)
                    {
                        var tagged = tagger.tagSentence(sentence).ToString();
                        var taggedInstructions = caseIdTaggedSteps[testCaseId];
                        taggedInstructions.Add(FetchNounAndVerbsOnly(tagged.Substring(1, tagged.Length - 2)));
                        caseIdTaggedSteps[testCaseId] = taggedInstructions;
                    }
                }
                Console.WriteLine("Progress: Line=" + ++lineCount / content.Length * 100);
            }
            foreach (var version in versions)
            {
                var testCases = from test in caseIdSteps where test.Key.Contains(version) select test.Key;
                var fileString = new StringBuilder();
                foreach (var testCase in testCases)
                {
                    var raw = caseIdSteps[testCase];
                    var tagged = caseIdTaggedSteps[testCase];
                    fileString.Append("Test Case ID: " + testCase.Split(':')[1] + Environment.NewLine);
                    foreach (var step in tagged)
                    {
                        fileString.Append(step + Environment.NewLine);
                    }
                    fileString.Append(Environment.NewLine + Environment.NewLine);
                }
                File.WriteAllText(directoryToSaveTestCases + ToPascalCase(product == "Tablet" ? "tablet" : "mobile") + @"_" + Regex.Match(version, @"\d+").Value + "_Rapid-tagged.txt", fileString.ToString());
            }
        }

        #endregion

        /// <summary>
        /// STEP 2: Compiling and saving results
        /// </summary>
        #region Compile New Version Results
        public void CompileResultsMySQL()
        {
            var versions = new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
            foreach (var version in versions)
            {
                var content = File.ReadAllLines(@"C:\Temp\SEALab\NLP-Project\MySqlTests-NewVersions\version@@-steps.csv".Replace("@@", version.ToString()));
                List<int> testIds = new List<int>();
                foreach (var line in content)
                {
                    int temp = 0;
                    if (!string.IsNullOrEmpty(line) && line.Split(',').Length > 1 && int.TryParse(line.Split(',')[1], out temp))
                    {
                        testIds.Add(Convert.ToInt32(line.Split(',')[1]));
                    }
                }

                var resultContent = File.ReadAllLines(@"C:\Temp\SEALab\NLP-Project\MySqlTests-NewVersions\version@@-results.csv".Replace("@@", version.ToString()));
                Dictionary<int, bool> testResult = new Dictionary<int, bool>();
                foreach (var line in resultContent)
                {
                    var thisId = Convert.ToInt32(line.Split(',')[1]);
                    var thisResult = line.Split(',')[2] == "passed" ? true : false;
                    if (!testResult.Any(x => x.Key == thisId))
                    {
                        testResult.Add(thisId, thisResult);
                    }
                    else
                    {
                        var oldResult = testResult.FirstOrDefault(x => x.Key == thisId).Value;
                        testResult[thisId] = oldResult & thisResult;
                    }
                }
                File.WriteAllLines(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\mobile_" + version + "_Result.txt", testResult.Select(x => "[" + x.Key + " " + x.Value + "]").ToArray());

            }
        }
        public void CompileResultsCombinedCSVMySQL(string product)
        {
            var versions = new string[14];
            if (product == "Tablet")
                versions = new string[] { "16 for Tablets", "17 Tablets", "18 Tablets", "19 Tablets", "20 Tablets", "21 Tablet", "22 Tablet", "23 Tablet", "24 Tablet", "25 Tablet", "26 Tablet", "27 Tablet", "28 Tablet", "29 Tablet" };
            else
                versions = new string[] { "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29" };
            var resultFile = File.ReadAllLines(@"C:\Temp\SEALab\NLP-Project\MySqlTests-NewVersions\" + (product == "Tablet" ? "tablets" : "mobile") + "_steps_result.csv");
            var testFile = File.ReadAllLines(@"C:\Temp\SEALab\NLP-Project\MySqlTests-NewVersions\" + (product == "Tablet" ? "tablets" : "mobile") + "_steps_view.csv");

            Dictionary<int, bool> testResult = new Dictionary<int, bool>();
            List<int> testIds = new List<int>();
            foreach (var version in versions)
            {
                testResult = new Dictionary<int, bool>();
                testIds = new List<int>();
                var lines = resultFile.Where(x => x.Contains(version));
                foreach (var line in lines)
                {
                    var parts = line.Split(new string[] { "\",\"" }, StringSplitOptions.None);
                    if (!testResult.Any(x => x.Key == Convert.ToInt32(parts[1])))
                    {
                        testResult.Add(Convert.ToInt32(parts[1]), false);
                    }
                }
                lines = testFile.Where(x => x.Contains(version));
                foreach (var line in lines)
                {
                    var parts = line.Split(new string[] { "\",\"" }, StringSplitOptions.None);
                    if (parts.Length > 1)
                    {
                        testIds.Add(Convert.ToInt32(parts[1]));
                    }
                }
                testIds = testIds.GroupBy(x => x).Select(x => x.First()).ToList();
                foreach (var id in testIds)
                {
                    if (!testResult.Any(x => x.Key == id))
                    {
                        testResult.Add(id, true);
                    }
                }
                File.WriteAllLines(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + (product == "Tablet" ? "tablet" : "mobile") + "_" + Regex.Match(version, @"\d+").Value + "_Result.txt", testResult.Select(x => "[" + x.Key + " " + x.Value + "]").ToArray());
            }
        }
        #endregion

        /// <summary>
        /// STEP 3: Make combinations of words {Nouns, Verbs, Nouns&VerbsAsInSequence, (Verb,Noun) pairs}
        /// </summary>
        /// <param name="product"></param>
        #region Combination of Words
        public void MakeNounVerbPairsForMySQL(string product)
        {
            var versions = new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
            var genericFileNameToAbstract = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_Rapid-tagged.txt";
            foreach (var v in versions)
            {
                string[] verbTags = { "VV", "VB", "VBD", "VBG", "VBN", "VBP", "VBZ" };
                var contents = File.ReadAllText(genericFileNameToAbstract.Replace("@@", v.ToString()));
                var finalString = new StringBuilder();
                int currentTestCaseID = -1;
                var allPairs = new List<string>();
                var testCase_NounVerbPair = new List<KeyValuePair<int, List<string>>>();
                var totalLines = contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Count();
                var passedLines = 0;
                foreach (var line in contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var currentTestCaseHeader = "";
                    //IF line is not starting of a test case
                    if (!line.Contains("Test Case ID: "))
                    {
                        //stepLevel++;
                        string previousTag = "";
                        string previousWord = "";
                        bool firstWord = true;
                        foreach (var taggedWord in line.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Contains('/')))
                        {
                            string currentWord = taggedWord.Split('/')[0];
                            string currentTag = taggedWord.Split('/')[1];
                            //IF this word is a verb, then store the word as previous word (to be used as the first word in a pair)
                            if (verbTags.Contains(currentTag) || firstWord)
                            {
                                previousTag = currentTag;
                                previousWord = currentWord;
                                firstWord = false;
                            }
                            else
                            {
                                //ELSE it can only be a noun, so make a pair using the previous word, and add it in the list AllPairs and finalString
                                //AND also store it in the matrix (KeyValue pair of (testcase,List of pairs))
                                if (!string.IsNullOrEmpty(previousWord))
                                {
                                    var pair = "" + previousWord.ToLower() + "," + currentWord.ToLower() /*+ stepLevel*/;
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
                    Console.Clear();
                    Console.Out.WriteLine((float)passedLines / (float)totalLines * 100 + "%");
                }
                List<string> distinctPairs = new List<string>();
                distinctPairs.AddRange(allPairs.Distinct());
                //Save the pairs and testcase IDs with pairs
                WriteUniquePairsAndTestCasesWithPairsForMySQL(distinctPairs, testCase_NounVerbPair, null, v.ToString(), product);
            }
        }
        public void MakeNounsVerbsCombinedForMySQL(string product)
        {
            var versions = new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
            var genericFileNameToAbstract = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_Rapid-tagged.txt";
            foreach (var v in versions)
            {
                //string[] verbTags = { "VV", "VB", "VBD", "VBG", "VBN", "VBP", "VBZ" };
                var contents = File.ReadAllText(genericFileNameToAbstract.Replace("@@", v.ToString()));
                var finalString = new StringBuilder();
                int currentTestCaseID = -1;
                var allWords = new List<string>();
                var testCase_NounsVerbs = new List<KeyValuePair<int, List<string>>>();
                var totalLines = contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Count();
                var passedLines = 0;
                foreach (var line in contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var currentTestCaseHeader = "";
                    //IF line is not starting of a test case
                    if (!line.Contains("Test Case ID: "))
                    {
                        ////stepLevel++;
                        //string previousTag = "";
                        //string previousWord = "";
                        //bool firstWord = true;
                        foreach (var taggedWord in line.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Contains('/')))
                        {
                            string currentWord = StripSpecialCharacters(taggedWord.Split('/')[0].ToLower());
                            //ELSE it can only be a noun, so make a pair using the previous word, and add it in the list AllPairs and finalString
                            //AND also store it in the matrix (KeyValue pair of (testcase,List of pairs))
                            if (!string.IsNullOrEmpty(currentWord))
                            {
                                allWords.Add(currentWord);
                                finalString.Append(currentWord + " ");
                                testCase_NounsVerbs.FirstOrDefault(x => x.Key == currentTestCaseID).Value.Add(currentWord);
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
                        testCase_NounsVerbs.Add(new KeyValuePair<int, List<string>>(currentTestCaseID, new List<string>()));
                        currentTestCaseHeader = line;
                        finalString.Append(currentTestCaseHeader + "\n");
                    }
                    finalString.Append("\n");
                    passedLines++;
                    Console.Clear();
                    Console.Out.WriteLine((float)passedLines / (float)totalLines * 100 + "%");
                }
                //Save the pairs and testcase IDs with pairs
                WriteTestCasesWithNounsVerbsForMySQL(testCase_NounsVerbs, null, v.ToString(), product);
            }
        }
        public void MakeNounsOrVerbsForMySQL(string product, string nounsOrVerbsCapital)
        {
            string[] nounTags = { "NN", "NNS", "NNP", "NNPS" };
            string[] verbTags = { "VV", "VB", "VBD", "VBG", "VBN", "VBP", "VBZ" };
            var versions = new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
            var genericFileNameToAbstract = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_Rapid-tagged.txt";
            foreach (var v in versions)
            {
                //string[] verbTags = { "VV", "VB", "VBD", "VBG", "VBN", "VBP", "VBZ" };
                var contents = File.ReadAllText(genericFileNameToAbstract.Replace("@@", v.ToString()));
                var finalString = new StringBuilder();
                int currentTestCaseID = -1;
                var allWords = new List<string>();
                var testCase_NounsVerbs = new List<KeyValuePair<int, List<string>>>();
                var totalLines = contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Count();
                var passedLines = 0;
                foreach (var line in contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var currentTestCaseHeader = "";
                    //IF line is not starting of a test case
                    if (!line.Contains("Test Case ID: "))
                    {
                        ////stepLevel++;
                        //string previousTag = "";
                        //string previousWord = "";
                        //bool firstWord = true;
                        foreach (var taggedWord in line.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Contains('/')))
                        {
                            string currentWord = StripSpecialCharacters(taggedWord.Split('/')[0].ToLower());
                            string currentTag = taggedWord.Split('/')[1];
                            //ELSE it can only be a noun, so make a pair using the previous word, and add it in the list AllPairs and finalString
                            //AND also store it in the matrix (KeyValue pair of (testcase,List of pairs))
                            if (!string.IsNullOrEmpty(currentWord))
                            {
                                if ((nounsOrVerbsCapital.ToLower().Contains("noun") && nounTags.Contains(currentTag)) || (nounsOrVerbsCapital.ToLower().Contains("verb") && verbTags.Contains(currentTag)))
                                {
                                    allWords.Add(currentWord);
                                    finalString.Append(currentWord + " ");
                                    testCase_NounsVerbs.FirstOrDefault(x => x.Key == currentTestCaseID).Value.Add(currentWord);
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
                        testCase_NounsVerbs.Add(new KeyValuePair<int, List<string>>(currentTestCaseID, new List<string>()));
                        currentTestCaseHeader = line;
                        finalString.Append(currentTestCaseHeader + "\n");
                    }
                    finalString.Append("\n");
                    passedLines++;
                    Console.Clear();
                    Console.Out.WriteLine((float)passedLines / (float)totalLines * 100 + "%");
                }
                //Save the pairs and testcase IDs with pairs
                WriteTestCasesWithNounsOrVerbsForMySQL(testCase_NounsVerbs, null, v.ToString(), product, nounsOrVerbsCapital);
            }
        }
        public void WriteTestCasesWithNounsVerbsForMySQL(List<KeyValuePair<int, List<string>>> testId_pair, string fileName, string version, string product)
        {
            var genericFileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_TestIdWithNounsVerbs.txt".Replace("@@", version);
            var tests = File.ReadAllLines(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_Rapid-Tagged.txt".Replace("@@", version.ToString()));
            var results = File.ReadAllLines(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_Result.txt".Replace(@"@@", version.ToString()));
            var r = new Dictionary<int, bool>();
            List<int> t = new List<int>();
            var compiledResults = new Dictionary<int, bool>();
            foreach (var line in results)
            {
                var temp = line.Replace("[", "").Replace("]", "").Split(' ');
                r.Add(Convert.ToInt32(temp[0]), Convert.ToBoolean(temp[1]));
            }
            foreach (var line in tests.Where(x => x.Contains("Test Case ID: ")))
            {
                t.Add(Convert.ToInt32(line.Substring(line.IndexOf("ID:") + 3)));
            }
            foreach (var test in t)
            {
                if (r.Any(x => x.Key == test))
                {
                    var temp = r.Where(x => x.Key == test);
                    compiledResults.Add(temp.FirstOrDefault().Key, temp.FirstOrDefault().Value);
                }
                else
                {
                    compiledResults.Add(test, true);
                }
            }
            StringBuilder passedFileString = new StringBuilder();
            foreach (var test in testId_pair)
            {
                var result = compiledResults.FirstOrDefault(x => x.Key == test.Key).Value;

                passedFileString.Append(test.Key + "=" + (compiledResults.FirstOrDefault(x => x.Key == test.Key).Value ? "Passed" : "Failed"));
                passedFileString.Append(Environment.NewLine);
                passedFileString.Append(string.Join(Environment.NewLine, test.Value));
                passedFileString.Append(Environment.NewLine);
                passedFileString.Append(Environment.NewLine);
            }
            File.WriteAllText(genericFileToSaveTestIdWithUniquePairs, passedFileString.ToString());
        }
        public void WriteTestCasesWithNounsOrVerbsForMySQL(List<KeyValuePair<int, List<string>>> testId_pair, string fileName, string version, string product, string nounsOrVerbsCapital)
        {
            var genericFileToSaveTestIdWithUniquePairs = (@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_TestIdWith" + ToPascalCase(nounsOrVerbsCapital) + "Only.txt").Replace("@@", version);
            var tests = File.ReadAllLines(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_Rapid-Tagged.txt".Replace("@@", version.ToString()));
            var results = File.ReadAllLines(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_Result.txt".Replace(@"@@", version.ToString()));
            var r = new Dictionary<int, bool>();
            List<int> t = new List<int>();
            var compiledResults = new Dictionary<int, bool>();
            foreach (var line in results)
            {
                var temp = line.Replace("[", "").Replace("]", "").Split(' ');
                r.Add(Convert.ToInt32(temp[0]), Convert.ToBoolean(temp[1]));
            }
            foreach (var line in tests.Where(x => x.Contains("Test Case ID: ")))
            {
                t.Add(Convert.ToInt32(line.Substring(line.IndexOf("ID:") + 3)));
            }
            foreach (var test in t)
            {
                if (r.Any(x => x.Key == test))
                {
                    var temp = r.Where(x => x.Key == test);
                    compiledResults.Add(temp.FirstOrDefault().Key, temp.FirstOrDefault().Value);
                }
                else
                {
                    compiledResults.Add(test, true);
                }
            }
            StringBuilder passedFileString = new StringBuilder();
            foreach (var test in testId_pair)
            {
                var result = compiledResults.FirstOrDefault(x => x.Key == test.Key).Value;

                passedFileString.Append(test.Key + "=" + (compiledResults.FirstOrDefault(x => x.Key == test.Key).Value ? "Passed" : "Failed"));
                passedFileString.Append(Environment.NewLine);
                passedFileString.Append(string.Join(Environment.NewLine, test.Value));
                passedFileString.Append(Environment.NewLine);
                passedFileString.Append(Environment.NewLine);
            }
            File.WriteAllText(genericFileToSaveTestIdWithUniquePairs, passedFileString.ToString());
        }
        public void WriteUniquePairsAndTestCasesWithPairsForMySQL(List<string> uniquePairs, List<KeyValuePair<int, List<string>>> testId_pair, string fileName, string version, string product)
        {
            var genericFileToSaveTestIdWithUniquePairs = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_TestIdWithUniquePair.txt".Replace("@@", version);
            var tests = File.ReadAllLines(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_Rapid-Tagged.txt".Replace("@@", version.ToString()));
            var results = File.ReadAllLines(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_Result.txt".Replace(@"@@", version.ToString()));
            var r = new Dictionary<int, bool>();
            List<int> t = new List<int>();
            var compiledResults = new Dictionary<int, bool>();
            foreach (var line in results)
            {
                var temp = line.Replace("[", "").Replace("]", "").Split(' ');
                r.Add(Convert.ToInt32(temp[0]), Convert.ToBoolean(temp[1]));
            }
            foreach (var line in tests.Where(x => x.Contains("Test Case ID: ")))
            {
                t.Add(Convert.ToInt32(line.Substring(line.IndexOf("ID:") + 3)));
            }
            foreach (var test in t)
            {
                if (r.Any(x => x.Key == test))
                {
                    var temp = r.Where(x => x.Key == test);
                    compiledResults.Add(temp.FirstOrDefault().Key, temp.FirstOrDefault().Value);
                }
                else
                {
                    compiledResults.Add(test, true);
                }
            }
            StringBuilder passedFileString = new StringBuilder();
            foreach (var test in testId_pair)
            {
                var result = compiledResults.FirstOrDefault(x => x.Key == test.Key).Value;

                passedFileString.Append(test.Key + "=" + (compiledResults.FirstOrDefault(x => x.Key == test.Key).Value ? "Passed" : "Failed"));
                passedFileString.Append(Environment.NewLine);
                passedFileString.Append(string.Join(Environment.NewLine, test.Value));
                passedFileString.Append(Environment.NewLine);
                passedFileString.Append(Environment.NewLine);
            }
            File.WriteAllText(genericFileToSaveTestIdWithUniquePairs, passedFileString.ToString());
        }
        #endregion
        #endregion

        /// <summary>
        /// STEP 4: Convert text vectors of test cases to binary vectors, based on indexing of unique Nouns,Verbs,VerbNoun_Pairs or NounsVerbsCombined
        /// such as if Test 1 = "Open Close", Open = 1, Close = 3, the resulting vector will be 1 0 1 0 0 0 0 .... the length of the vectors is the length of biggest vector, appended 0 for remaining indices.
        /// </summary>
        /// <param name="product"></param>
        #region Test Representation to Binary Vectors
        public void ProcessAllPairsInALitmus(string product)
        {
            //var versions = new int[] { 10, 11, 12, 13, 30, 35, 36, 40, 50, 60, 70, 80, 90 };
            var versions = new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
            var countVersionsProcessed = 0.0;
            //var folderNameToSaveAllPairFile = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\CPC\Pairs\";
            var folderNameToSaveAllPairFile = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\CPC\" + ToPascalCase(product) + "Pairs\\";
            Dictionary<string, int> indexedPairs = new Dictionary<string, int>();
            var index = 0;
            foreach (var v in versions)
            {
                var genericFileNameToReadLitmusFile = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_TestIdWithUniquePair.txt".Replace("@@", v.ToString());
                var content = File.ReadAllLines(genericFileNameToReadLitmusFile);
                var countLinesProcessed = 0.0;
                foreach (var line in content)
                {
                    if (!line.Contains("=Passed") && !line.Contains("=Failed") && !string.IsNullOrEmpty(line))
                    {
                        if (!indexedPairs.Any(x => x.Key == line.Trim()))
                        {
                            indexedPairs.Add(line.Trim(), ++index);
                        }
                    }
                    Console.WriteLine("Progress: Lines Processed=" + countLinesProcessed++ / content.Length * 100 + "%;\tVersions Processed=" + countVersionsProcessed / versions.Length * 100 + "%");

                }

                countVersionsProcessed++;
            }

            if (!Directory.Exists(folderNameToSaveAllPairFile))
                Directory.CreateDirectory(folderNameToSaveAllPairFile);
            File.WriteAllLines(folderNameToSaveAllPairFile + @"AllIndexed.txt", indexedPairs.Select(x => x.Value + " " + x.Key).ToArray());
        }
        public void IndexAllPairsInLitmus(string product)
        {
            var indicesWithPairs = File.ReadAllLines(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\CPC\" + ToPascalCase(product) + @"Pairs\AllIndexed.txt");
            Dictionary<string, int> indexedPairs = new Dictionary<string, int>();
            foreach (var line in indicesWithPairs)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    var temp = line.Split(' ');
                    indexedPairs.Add(temp[1], Convert.ToInt32(temp[0]));
                }
            }
            //var versions = new int[] { 10, 11, 12, 13, 30, 35, 36, 40, 50, 60, 70, 80, 90 };
            var versions = new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
            var countVersionsProcessed = 0.0;
            foreach (var v in versions)
            {
                var genericFileNameToReadLitmusFile = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + @"_@@_TestIdWithUniquePair.txt".Replace("@@", v.ToString());
                var genericFolderNameToSaveIndexPairFiles = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\CPC\" + ToPascalCase(product) + @"Pairs\@@\".Replace("@@", v.ToString());
                var content = File.ReadAllLines(genericFileNameToReadLitmusFile);
                string lastTestHeader = "";
                string lastTestCaseIndexed = "";
                if (!Directory.Exists(genericFolderNameToSaveIndexPairFiles))
                    Directory.CreateDirectory(genericFolderNameToSaveIndexPairFiles);
                var countLinesProcessed = 0.0;
                foreach (var line in content)
                {
                    if (!line.Contains("=Passed") && !line.Contains("=Failed") && !string.IsNullOrEmpty(line))
                    {
                        lastTestCaseIndexed += indexedPairs[line.ToLower().Trim()] + " ";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(lastTestHeader) && !string.IsNullOrEmpty(lastTestCaseIndexed.Trim()))
                        {
                            var temp = Enumerable.Repeat(0, indexedPairs.Count).ToArray(); ;
                            foreach (var pairIndex in lastTestCaseIndexed.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                temp[Convert.ToInt32(pairIndex) - 1] = 1;
                            }
                            //File.WriteAllText(genericFolderNameToSaveIndexPairFiles + @"\" + lastTestHeader + ".txt", lastTestCaseIndexed.Trim());
                            File.WriteAllText(genericFolderNameToSaveIndexPairFiles + @"\" + lastTestHeader + ".txt", string.Join(" ", temp));
                        }
                        lastTestHeader = line;
                        lastTestCaseIndexed = "";
                    }
                    Console.WriteLine("Progress: Lines Processed=" + countLinesProcessed++ / content.Length * 100 + "%;\tVersions Processed=" + countVersionsProcessed / versions.Length * 100 + "%");

                }

                countVersionsProcessed++;
            }
        }
        public void ProcessAllNounsOrVerbsOrNounsVerbsInALitmus(string product, string nounsOnlyOrVerbsOnlyOrNounsVerbs)
        {
            //var versions = new int[] { 10, 11, 12, 13, 30, 35, 36, 40, 50, 60, 70, 80, 90 };
            var versions = new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
            var countVersionsProcessed = 0.0;
            //var folderNameToSaveAllPairFile = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\CPC\Pairs\";
            var folderNameToSaveAllPairFile = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\CPC\" + ToPascalCase(product) + nounsOnlyOrVerbsOnlyOrNounsVerbs + "\\"; //"NounsVerbs\\";
            Dictionary<string, int> indexedPairs = new Dictionary<string, int>();
            var index = 0;
            foreach (var v in versions)
            {
                var genericFileNameToReadLitmusFile = (@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_TestIdWith" + nounsOnlyOrVerbsOnlyOrNounsVerbs + ".txt").Replace("@@", v.ToString());
                var content = File.ReadAllLines(genericFileNameToReadLitmusFile);
                var countLinesProcessed = 0.0;
                foreach (var line in content)
                {
                    if (!line.Contains("=Passed") && !line.Contains("=Failed") && !string.IsNullOrEmpty(line))
                    {
                        if (!indexedPairs.Any(x => x.Key == line.Trim()))
                        {
                            indexedPairs.Add(line.Trim(), ++index);
                        }
                    }
                    Console.WriteLine("Progress: Lines Processed=" + countLinesProcessed++ / content.Length * 100 + "%;\tVersions Processed=" + countVersionsProcessed / versions.Length * 100 + "%");

                }

                countVersionsProcessed++;
            }

            if (!Directory.Exists(folderNameToSaveAllPairFile))
                Directory.CreateDirectory(folderNameToSaveAllPairFile);
            File.WriteAllLines(folderNameToSaveAllPairFile + @"AllIndexed.txt", indexedPairs.Select(x => x.Value + " " + x.Key).ToArray());
        }
        public void IndexAllNounsOrVerbsOrNounsVerbsInLitmus(string product, string nounsOnlyOrVerbsOnlyOrNounsVerbs)
        {
            var indicesWithPairs = File.ReadAllLines(@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\CPC\" + ToPascalCase(product) + nounsOnlyOrVerbsOnlyOrNounsVerbs + @"\AllIndexed.txt"); //@"NounsVerbs\AllIndexed.txt");
            Dictionary<string, int> indexedPairs = new Dictionary<string, int>();
            foreach (var line in indicesWithPairs)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    var temp = line.Split(' ');
                    indexedPairs.Add(temp[1], Convert.ToInt32(temp[0]));
                }
            }
            //var versions = new int[] { 10, 11, 12, 13, 30, 35, 36, 40, 50, 60, 70, 80, 90 };
            var versions = new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
            var countVersionsProcessed = 0.0;
            foreach (var v in versions)
            {
                var genericFileNameToReadLitmusFile = (@"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + @"_@@_TestIdWith" + nounsOnlyOrVerbsOnlyOrNounsVerbs + ".txt").Replace("@@", v.ToString());//@"_@@_TestIdWithNounsVerbs.txt".Replace("@@", v.ToString());
                var genericFolderNameToSaveIndexPairFiles = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\CPC\" + ToPascalCase(product) + nounsOnlyOrVerbsOnlyOrNounsVerbs + @"\@@\".Replace("@@", v.ToString());//@"NounsVerbs\@@\".Replace("@@", v.ToString());
                var content = File.ReadAllLines(genericFileNameToReadLitmusFile);
                string lastTestHeader = "";
                string lastTestCaseIndexed = "";
                if (!Directory.Exists(genericFolderNameToSaveIndexPairFiles))
                    Directory.CreateDirectory(genericFolderNameToSaveIndexPairFiles);
                var countLinesProcessed = 0.0;
                foreach (var line in content)
                {
                    if (!line.Contains("=Passed") && !line.Contains("=Failed") && !string.IsNullOrEmpty(line))
                    {
                        lastTestCaseIndexed += indexedPairs[line.ToLower().Trim()] + " ";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(lastTestHeader) && !string.IsNullOrEmpty(lastTestCaseIndexed.Trim()))
                        {
                            var temp = Enumerable.Repeat(0, indexedPairs.Count).ToArray(); ;
                            foreach (var pairIndex in lastTestCaseIndexed.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                temp[Convert.ToInt32(pairIndex) - 1] = 1;
                            }
                            //File.WriteAllText(genericFolderNameToSaveIndexPairFiles + @"\" + lastTestHeader + ".txt", lastTestCaseIndexed.Trim());
                            File.WriteAllText(genericFolderNameToSaveIndexPairFiles + @"\" + lastTestHeader + ".txt", string.Join(" ", temp));
                        }
                        lastTestHeader = line;
                        lastTestCaseIndexed = "";
                    }
                    Console.WriteLine("Progress: Lines Processed=" + countLinesProcessed++ / content.Length * 100 + "%;\tVersions Processed=" + countVersionsProcessed / versions.Length * 100 + "%");

                }

                countVersionsProcessed++;
            }
        }
        #endregion

        /// <summary>
        /// STEP 5: Order test suites, and Report APFDs
        /// OrderForNounVerbPairs: Orders for (Verb,Noun) pairs
        /// OrderForNounsVerbs:    Orders for Nouns, Verbs and Nouns&VerbsAsInSequence
        /// OrderForStringDistanceNounVerbPairs: Orders for text diversity
        /// </summary>
        /// <param name="product"></param>
        /// <param name="frequencyOrAdditionalGreedy"></param>

        #region New Code for Ordering Test Cases
        public void OrderForNounVerbPairs(string product, string frequencyOrAdditionalGreedy)
        {
            var versions = new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
            var path = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_TestIdWithUniquePair.txt";
            var resultPath = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_Result.txt";
            var apfdPath = @"C:\Temp\" + product + "-NounVerbPairs-" + frequencyOrAdditionalGreedy + "-Apfds.txt";
            File.WriteAllText(apfdPath, "");
            foreach (var version in versions)
            {
                //var version = versions[0];
                var content = File.ReadAllLines(path.Replace("@@", version.ToString()));
                var pairDictionaryWithCount = new Dictionary<string, int>();
                var testIdPairs = new Dictionary<int, List<string>>();
                //Unique pairs with their counts in the version
                var testId = 0;
                var testPairs = new List<string>();
                var lineCount = 0.0;
                foreach (var line in content.Where(x => !string.IsNullOrEmpty(x)))
                {
                    var l = line.ToLower().Trim();
                    if (!string.IsNullOrEmpty(l) && !l.Contains("passed") && !l.Contains("failed"))
                    {
                        testPairs.Add(l);
                        if (pairDictionaryWithCount.Any(x => x.Key == l))
                        {
                            pairDictionaryWithCount[l]++;
                        }
                        else
                        {
                            pairDictionaryWithCount.Add(l, 1);
                        }
                    }
                    else
                    {
                        if (testId != 0)
                        {
                            if (!testIdPairs.Any(x => x.Key == testId))
                            {
                                testIdPairs.Add(testId, testPairs);
                            }
                        }
                        testId = Convert.ToInt32(l.Split('=')[0]);
                        testPairs = new List<string>();
                    }
                    Console.WriteLine("Progress: " + (lineCount++) / content.Length * 100);
                }
                pairDictionaryWithCount = pairDictionaryWithCount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                testIdPairs = testIdPairs.OrderByDescending(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);
                for (var iteration = 0; iteration < 10; iteration++)
                {


                    var MyRank = new Dictionary<int, int>();

                    if (frequencyOrAdditionalGreedy.ToLower() == "frequency")
                    {
                        foreach (var pair in pairDictionaryWithCount)
                        {
                            var testIds = testIdPairs.Where(x => x.Value.Contains(pair.Key));
                            if (testIds.Count() == 0)
                                continue;
                            var index = 0;
                            if (testIds.Count() > iteration)
                            { index = iteration; }
                            else
                            { index = iteration % testIds.Count(); }

                            for (var i = index; i >= 0; i--)
                            {
                                var temp = testIds.ElementAt(i);
                                if (!MyRank.Any(x => x.Key == temp.Key))
                                {
                                    MyRank.Add(temp.Key, 0);
                                    break;
                                }
                            }

                        }


                    }
                    else
                    {

                        var tempTestIdPairs = new Dictionary<int, List<string>>();
                        foreach (var testIdPair in testIdPairs)
                        {
                            tempTestIdPairs.Add(testIdPair.Key, testIdPair.Value);
                        }

                        for (var a = iteration; tempTestIdPairs.Count > 0;)
                        {
                            if (a >= tempTestIdPairs.Count)
                                a = tempTestIdPairs.Count - 1;
                            var currentTest = tempTestIdPairs.ElementAt(a);
                            var pairsInThisTestCase = currentTest.Value;
                            for (var j = 0; j < pairsInThisTestCase.Count; j++)
                            {
                                var pair = pairsInThisTestCase.ElementAt(j);
                                for (var i = 0; i < tempTestIdPairs.Count; i++)
                                {
                                    if (tempTestIdPairs.ElementAt(i).Value.Contains(pair))
                                    {
                                        tempTestIdPairs[tempTestIdPairs.ElementAt(i).Key].Remove(pair);
                                    }
                                }
                            }

                            MyRank.Add(currentTest.Key, 0);

                            tempTestIdPairs.Remove(currentTest.Key);
                            tempTestIdPairs = tempTestIdPairs.OrderByDescending(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);
                        }
                    }



                    //Getting Results
                    var results = File.ReadAllLines(resultPath.Replace("@@", version.ToString()));
                    var r = new Dictionary<int, bool>();
                    foreach (var line in results)
                    {
                        var temp = line.Replace("[", "").Replace("]", "").Split(' ');
                        r.Add(Convert.ToInt32(temp[0]), Convert.ToBoolean(temp[1]));
                    }



                    var myAPFD = 1.0;
                    var progress = 0.0;
                    var totalFaults = r.Count(x => x.Value == false);
                    var indicesOfFailedTests = new List<int>();
                    double[] apfdPrioritizedForIteration = new double[MyRank.Count()];
                    var indexDetectingFault = 0.0;
                    for (var index = 0; index < MyRank.Count(); index++)
                    {
                        var thisTest = r.FirstOrDefault(x => x.Key == MyRank.ElementAt(index).Key);
                        if (thisTest.Key != 0 && thisTest.Value == false)
                        {
                            indexDetectingFault += index;
                            indicesOfFailedTests.Add(index);
                        }
                        Console.Out.WriteLine("Progress: " + progress / MyRank.Count + "%, APFD: " + myAPFD);
                        apfdPrioritizedForIteration[Convert.ToInt32(progress)] = myAPFD;
                        progress++;
                    }
                    myAPFD = 1 - (indexDetectingFault / (totalFaults * MyRank.Count)) + (1 / (float)(2 * MyRank.Count));
                    File.AppendAllText(apfdPath, Environment.NewLine + "Version: " + version + ", APFD: " + myAPFD + ", Iteration: " + iteration);

                }

            }




        }
        public void OrderForNounsVerbs(string product, string nounsOrVerbs, string frequencyMethodOrAddGreedy)
        {
            var versions = new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
            string nv = "NounsVerbs";
            if (nounsOrVerbs.ToLower().Contains("noun"))
                nv = "NounsOnly";
            else if (nounsOrVerbs.ToLower().Contains("verb"))
                nv = "VerbsOnly";

            var path = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_TestIdWith" + nv + ".txt";
            var resultPath = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_Result.txt";
            var apfdPath = @"C:\Temp\" + product + "-" + nv + "-" + frequencyMethodOrAddGreedy + "-Apfds.txt";
            File.WriteAllText(apfdPath, "");
            foreach (var version in versions)
            {
                //var version = versions[0];
                var content = File.ReadAllLines(path.Replace("@@", version.ToString()));
                var pairDictionaryWithCount = new Dictionary<string, int>();
                var testIdPairs = new Dictionary<int, List<string>>();
                //Unique pairs with their counts in the version
                var testId = 0;
                var testPairs = new List<string>();
                var lineCount = 0.0;
                foreach (var line in content.Where(x => !string.IsNullOrEmpty(x)))
                {
                    var l = line.ToLower().Trim();
                    if (!string.IsNullOrEmpty(l) && !l.Contains("passed") && !l.Contains("failed"))
                    {
                        testPairs.Add(l);
                        if (pairDictionaryWithCount.Any(x => x.Key == l))
                        {
                            pairDictionaryWithCount[l]++;
                        }
                        else
                        {
                            pairDictionaryWithCount.Add(l, 1);
                        }
                    }
                    else
                    {
                        if (testId != 0)
                        {
                            if (!testIdPairs.Any(x => x.Key == testId))
                            {
                                testIdPairs.Add(testId, testPairs);
                            }
                        }
                        testId = Convert.ToInt32(l.Split('=')[0]);
                        testPairs = new List<string>();
                    }
                    Console.WriteLine("Progress: " + (lineCount++) / content.Length * 100);
                }
                pairDictionaryWithCount = pairDictionaryWithCount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                testIdPairs = testIdPairs.OrderByDescending(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);

                for (var iteration = 0; iteration < 10; iteration++)
                {
                    var MyRank = new Dictionary<int, int>();


                    if (frequencyMethodOrAddGreedy.ToLower() == "frequency")
                    {
                        foreach (var pair in pairDictionaryWithCount)
                        {
                            var testIds = testIdPairs.Where(x => x.Value.Contains(pair.Key));
                            if (testIds.Count() == 0)
                                continue;
                            var index = 0;
                            if (testIds.Count() > iteration)
                            { index = iteration; }
                            else
                            { index = iteration % testIds.Count(); }

                            for (var i = index; i >= 0; i--)
                            {
                                var temp = testIds.ElementAt(i);
                                if (!MyRank.Any(x => x.Key == temp.Key))
                                {
                                    MyRank.Add(temp.Key, 0);
                                    break;
                                }
                            }

                        }
                    }
                    else
                    {

                        var tempTestIdPairs = new Dictionary<int, List<string>>();
                        foreach (var testIdPair in testIdPairs)
                        {
                            tempTestIdPairs.Add(testIdPair.Key, testIdPair.Value);
                        }

                        for (var a = iteration; tempTestIdPairs.Count > 0;)
                        {
                            if (a >= tempTestIdPairs.Count)
                                a = tempTestIdPairs.Count - 1;
                            var currentTest = tempTestIdPairs.ElementAt(a);
                            var pairsInThisTestCase = currentTest.Value;
                            for (var j = 0; j < pairsInThisTestCase.Count; j++)
                            {
                                var pair = pairsInThisTestCase.ElementAt(j);
                                for (var i = 0; i < tempTestIdPairs.Count; i++)
                                {
                                    if (tempTestIdPairs.ElementAt(i).Value.Contains(pair))
                                    {
                                        tempTestIdPairs[tempTestIdPairs.ElementAt(i).Key].Remove(pair);
                                    }
                                }
                            }

                            MyRank.Add(currentTest.Key, 0);

                            tempTestIdPairs.Remove(currentTest.Key);
                            tempTestIdPairs = tempTestIdPairs.OrderByDescending(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);
                        }
                    }


                    //Getting Results
                    var results = File.ReadAllLines(resultPath.Replace("@@", version.ToString()));
                    var r = new Dictionary<int, bool>();
                    foreach (var line in results)
                    {
                        var temp = line.Replace("[", "").Replace("]", "").Split(' ');
                        r.Add(Convert.ToInt32(temp[0]), Convert.ToBoolean(temp[1]));
                    }









                    var myAPFD = 1.0;
                    var progress = 0.0;
                    var totalFaults = r.Count(x => x.Value == false);
                    var indicesOfFailedTests = new List<int>();
                    double[] apfdPrioritizedForIteration = new double[MyRank.Count()];
                    var indexDetectingFault = 0.0;
                    for (var index = 0; index < MyRank.Count(); index++)
                    {
                        var thisTest = r.FirstOrDefault(x => x.Key == MyRank.ElementAt(index).Key);
                        if (thisTest.Key != 0 && thisTest.Value == false)
                        {
                            indexDetectingFault += index;
                            indicesOfFailedTests.Add(index);
                        }
                        Console.Out.WriteLine("Progress: " + progress / MyRank.Count + "%, APFD: " + myAPFD);
                        apfdPrioritizedForIteration[Convert.ToInt32(progress)] = myAPFD;
                        progress++;
                    }
                    myAPFD = 1 - (indexDetectingFault / (totalFaults * MyRank.Count)) + (1 / (float)(2 * MyRank.Count));
                    File.AppendAllText(apfdPath, Environment.NewLine + "Version: " + version + ", APFD: " + myAPFD + ", Iteration: " + iteration);

                }



            }


        }
        public void OrderForStringDistanceNounVerbPairs(string product, string combination, string distanceFormula)
        {
            var versions = new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
            var path = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\CPC\" + ToPascalCase(product) + ToPascalCase(combination) + "\\@@\\";
            var resultPath = @"C:\Temp\SEALab\NLP-Project\ExtractedTestCases\" + product + "_@@_Result.txt";
            var apfdPath = @"C:\Temp\" + product + "-" + combination + "-" + distanceFormula + "Distance-Apfds.txt";
            File.WriteAllText(apfdPath, "");
            foreach (var version in versions)
            {
                var testIdVector = new Dictionary<int, string>();
                var files = Directory.GetFiles(path.Replace("@@", version.ToString()));
                foreach (var file in files)
                {
                    testIdVector.Add(Convert.ToInt32(Path.GetFileName(file).Split('=')[0]), File.ReadAllText(file));
                }

                var distances = new Dictionary<int, Dictionary<int, double>>();
                var count = 0.0;
                foreach (var test1 in testIdVector)
                {
                    distances.Add(test1.Key, new Dictionary<int, double>());
                    var allExceptTest1 = testIdVector.Except(new List<KeyValuePair<int, string>>() { new KeyValuePair<int, string>(test1.Key, test1.Value) });
                    foreach (var test2 in allExceptTest1)
                    {
                        var temp = distances[test1.Key];
                        if (distanceFormula == "E")
                        {
                            temp.Add(test2.Key, EuclideanDistance(test1.Value, test2.Value));
                        }
                        else if (distanceFormula == "M")
                        {
                            temp.Add(test2.Key, ManhattanDistance(test1.Value, test2.Value));
                        }
                        else if (distanceFormula == "H")
                        {
                            temp.Add(test2.Key, HammingDistance(test1.Value, test2.Value));
                        }
                        else if (distanceFormula == "C")
                        {
                            temp.Add(test2.Key, ChebyshevDistance(test1.Value, test2.Value));
                        }
                        distances[test1.Key] = temp;
                        Console.WriteLine(count++ / (testIdVector.Count * testIdVector.Count) * 100 + "%");
                    }
                }



                for (var iteration = 0; iteration < 10; iteration++)
                {


                    var MyRank = new Dictionary<int, string>();
                    var tempList = new Dictionary<int, string>();
                    foreach (var test in testIdVector)
                    {
                        tempList.Add(test.Key, test.Value);
                    }

                    var biggestTestCase = tempList.OrderByDescending(x => x.Value.Split(' ').Count(y => y == "1")).ElementAt(iteration);
                    MyRank.Add(biggestTestCase.Key, biggestTestCase.Value);
                    tempList.Remove(biggestTestCase.Key);
                    while (tempList.Count > 0)
                    {
                        var distancesFromOtherTestCases = new Dictionary<int, double>();
                        foreach (var test in tempList)
                        {
                            var totalDistance = 0.0;
                            foreach (var rankedTestCase in MyRank)
                            {
                                //totalDistance += EuclideanDistance(rankedTestCase.Value, test.Value);
                                totalDistance += (distances[test.Key])[rankedTestCase.Key];
                            }
                            distancesFromOtherTestCases.Add(test.Key, totalDistance / MyRank.Count);
                        }

                        var farthestTestCase = distancesFromOtherTestCases.OrderByDescending(x => x.Value).FirstOrDefault();
                        MyRank.Add(farthestTestCase.Key, tempList[farthestTestCase.Key]);
                        tempList.Remove(farthestTestCase.Key);
                    }














                    //Getting Results
                    var results = File.ReadAllLines(resultPath.Replace("@@", version.ToString()));
                    var r = new Dictionary<int, bool>();
                    foreach (var line in results)
                    {
                        var temp = line.Replace("[", "").Replace("]", "").Split(' ');
                        r.Add(Convert.ToInt32(temp[0]), Convert.ToBoolean(temp[1]));
                    }









                    var myAPFD = 1.0;
                    var progress = 0.0;
                    var totalFaults = r.Count(x => x.Value == false);
                    var indicesOfFailedTests = new List<int>();
                    double[] apfdPrioritizedForIteration = new double[MyRank.Count()];
                    var indexDetectingFault = 0.0;
                    for (var index = 0; index < MyRank.Count(); index++)
                    {
                        var thisTest = r.FirstOrDefault(x => x.Key == MyRank.ElementAt(index).Key);
                        if (thisTest.Key != 0 && thisTest.Value == false)
                        {
                            indexDetectingFault += index;
                            indicesOfFailedTests.Add(index);
                        }
                        Console.Out.WriteLine("Progress: " + progress / MyRank.Count + "%, APFD: " + myAPFD);
                        apfdPrioritizedForIteration[Convert.ToInt32(progress)] = myAPFD;
                        progress++;
                    }
                    myAPFD = 1 - (indexDetectingFault / (totalFaults * MyRank.Count)) + (1 / (float)(2 * MyRank.Count));
                    File.AppendAllText(apfdPath, Environment.NewLine + "Version: " + version + ", APFD: " + myAPFD + ", Iteration: " + iteration);

                }

            }




        }
        #endregion
        

        #region Utility Methods
        private string ToPascalCase(string product)
        {
            string s = product[0].ToString().ToUpper();

            for (var a = 1; a < product.Length; a++)
            {
                s += product[a].ToString();
            }
            return s;
        }
        private string StripSpecialCharacters(string str)
        {
            char[] arr = str.ToCharArray();

            arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                              || char.IsWhiteSpace(c)
                                              || c == '-'
                                              || c == ' '
                                              || c == '_')));
            str = new string(arr);
            return str;
        }
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
        #region Distance Functions for Order by Distance (text diversity) method
        static double EuclideanDistance(string first, string second)
        {
            double[] x = first.Split(' ').ToList().ConvertAll(a => Convert.ToDouble(a)).ToArray();
            double[] y = second.Split(' ').ToList().ConvertAll(a => Convert.ToDouble(a)).ToArray();
            double Sum = 0;
            double distance = 0;
            if (x.Length != y.Length)
                throw new Exception("[ERROR]: Length is not equal.");
            int len = x.Length;
            for (int i = 0; i < len; i++)
            {
                Sum = Sum + Math.Pow((x[i] - y[i]), 2.0);
                distance = Math.Sqrt(Sum);
            }
            return distance;
        }
        static double ManhattanDistance(string first, string second)
        {
            double[] x = first.Split(' ').ToList().ConvertAll(a => Convert.ToDouble(a)).ToArray();
            double[] y = second.Split(' ').ToList().ConvertAll(a => Convert.ToDouble(a)).ToArray();
            double Sum = 0;
            if (x.Length != y.Length)
                throw new Exception("[ERROR]: Length is not equal.");
            int len = x.Length;
            for (int i = 0; i < len; i++)
            {
                Sum = Sum + Math.Abs(x[i] - y[i]);
            }
            return Sum;
        }
        static double HammingDistance(string first, string second)
        {
            double[] x = first.Split(' ').ToList().ConvertAll(a => Convert.ToDouble(a)).ToArray();
            double[] y = second.Split(' ').ToList().ConvertAll(a => Convert.ToDouble(a)).ToArray();
            double distance = 0;
            if (x.Length != y.Length)
                throw new Exception("[ERROR]: Length is not equal.");
            int len = x.Length;
            for (int i = 0; i < len; i++)
            {
                if (x[i] != y[i])
                {
                    distance++;
                }
            }
            return distance;
        }
        static double ChebyshevDistance(string first, string second)
        {
            double[] x = first.Split(' ').ToList().ConvertAll(a => Convert.ToDouble(a)).ToArray();
            double[] y = second.Split(' ').ToList().ConvertAll(a => Convert.ToDouble(a)).ToArray();
            double distance = 0;
            if (x.Length != y.Length)
                throw new Exception("[ERROR]: Length is not equal.");
            int len = x.Length;
            for (int i = 0; i < len; i++)
            {
                distance = Math.Abs((x[i] - y[i])) > distance ? Math.Abs((x[i] - y[i])) : distance;
            }
            return distance;
        }
        #endregion
        #region Custom random method for returning all random in a range at once
        static System.Random random = new System.Random();
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
        #endregion

    }
}
