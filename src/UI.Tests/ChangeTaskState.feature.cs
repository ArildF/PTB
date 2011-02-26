// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.3.5.2
//      Runtime Version:4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
namespace Rogue.Ptb.UI.Tests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.3.5.2")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Change task state")]
    public partial class ChangeTaskStateFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ChangeTaskState.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Change task state", "In order to properly reflect the actual state of tasks\r\nAs a user\r\nI want to chan" +
                    "ge the state of tasks", ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
            this.FeatureBackground();
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void FeatureBackground()
        {
#line 6
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Title"});
            table1.AddRow(new string[] {
                        "Do stuff"});
#line 7
testRunner.Given("that the following tasks already exist and are loaded:", ((string)(null)), table1);
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Set task started")]
        public virtual void SetTaskStarted()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Set task started", ((string[])(null)));
#line 11
this.ScenarioSetup(scenarioInfo);
#line 12
testRunner.When("I drag task #1 to the \"In Progress\" column");
#line 13
testRunner.Then("task #1 should be \"In Progress\"");
#line hidden
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Set task completed")]
        public virtual void SetTaskCompleted()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Set task completed", ((string[])(null)));
#line 16
this.ScenarioSetup(scenarioInfo);
#line 17
testRunner.When("I drag task #1 to the \"Complete\" column");
#line 18
testRunner.Then("task #1 should be \"Complete\"");
#line hidden
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Set task abandoned")]
        public virtual void SetTaskAbandoned()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Set task abandoned", ((string[])(null)));
#line 20
this.ScenarioSetup(scenarioInfo);
#line 21
testRunner.When("I drag task #1 to the \"Abandoned\" column");
#line 22
testRunner.Then("task #1 should be \"Abandoned\"");
#line hidden
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Drag back to not started")]
        public virtual void DragBackToNotStarted()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Drag back to not started", ((string[])(null)));
#line 24
this.ScenarioSetup(scenarioInfo);
#line 25
testRunner.When("I drag task #1 to the \"In Progress\" column");
#line 26
testRunner.And("I drag task #1 to the \"Not Started\" column");
#line 27
testRunner.Then("task #1 should be \"Not Started\"");
#line hidden
            testRunner.CollectScenarioErrors();
        }
    }
}
#endregion