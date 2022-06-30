using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Formulas;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
    public class NFormulasExample : NExampleBase
    {
        #region Constructors

        public NFormulasExample()
        {
        }
        static NFormulasExample()
        {
            NFormulasExampleSchema = NSchema.Create(typeof(NFormulasExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            m_FormulaEngine = new NFormulaEngine();

            NStackPanel stack = new NStackPanel();
			NDockLayout.SetDockArea(stack, ENDockArea.Center);

            m_InputTextBox = new NTextBox();
            stack.Add(m_InputTextBox);

            NStackPanel hstack = new NStackPanel();
            hstack.Direction = ENHVDirection.LeftToRight;
            stack.Add(hstack);

            NButton evaluateButton = new NButton("Evaluate");
            evaluateButton.Click += new Function<NEventArgs>(OnEvaluateButtonClick);
            hstack.Add(evaluateButton);

            NButton evaluateAllButton = new NButton("Evaluate All");
            evaluateAllButton.Click += new Function<NEventArgs>(OnEvaluateAllButtonClick);
            hstack.Add(evaluateAllButton);

            m_ResultTextBox = new NTextBox();
            stack.Add(m_ResultTextBox);

            return stack;
        }
        protected override NWidget CreateExampleControls()
        {
            NTreeView testsView = CreateTestsTreeView();
            return new NGroupBox("Predefined Examples", testsView);
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
Demonstrates the strong support for Formulas. Formula expressions can be assigned to any DOM Element property.
</p>
";
        }

        #endregion

        #region Implementation

        NTreeView CreateTestsTreeView()
        {
            NTreeViewItem categoryItem, folderItem;

            NTreeView treeView = new NTreeView();
            m_TestsTreeView = treeView;

            #region Operators
            
            folderItem = new NTreeViewItem("Operators");
            treeView.Items.Add(folderItem);

            #region Arithmetic Operators

            categoryItem = new NTreeViewItem("Arithmetic");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[] {
                "+10",
                "-10",
                "-ARRAY(10, 12)",
                "10 ^ 2",
                "ARRAY(10, 12) ^ 2",
                "10 * 2",
                "ARRAY(10, 12) * 2",
                "10 / 2",
                "ARRAY(10, 12) / 2",
                "10 + 2",
                "ARRAY(10, 12) + 2",
                "ARRAY(10, 12) + ARRAY(12, 23)",
                "10 + \"Nevron\"",
                "10 - 2",
                "ARRAY(10, 12) - 2",
                "ARRAY(10, 12) - ARRAY(12, 23)"
            });

            #endregion

            #region Comparision Operators

            categoryItem = new NTreeViewItem("Comparision");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[] {
               "10 > 2",
               "10 < 2",
               "10 >= 2",
               "10 >= 10",
               "10 <= 2",
               "10 <= 10",
               "10 == 2",
               "10 != 2"
            });

            #endregion

            #region Logical operators

            categoryItem = new NTreeViewItem("Logical");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[] {
               "true && false",
               "true || false",
               "!true"
            });

            #endregion

            #region Bitwise operators

            categoryItem = new NTreeViewItem("Bitwise");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[] {
               "7 & 2",
               "5 | 3",
               "~1"
            });

            #endregion

            #region Assignment operators

            categoryItem = new NTreeViewItem("Assignment");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[] {
                "a=5; b=3; a+b;",
                "a=5; a+=3",
                "a=5; a-=3"
            });

            #endregion

            #endregion

            #region Functions

            folderItem = new NTreeViewItem("Functions");
            treeView.Items.Add(folderItem);

            #region Bitwise 

            categoryItem = new NTreeViewItem("Bitwise");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[]{ 
                   "BITAND(7,2)",
                   "BITNOT(1)",
                   "BITOR(5,3)",
                   "BITXOR(5,3)",
            });
                
            #endregion

            #region Logical

            categoryItem = new NTreeViewItem("Logical");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[]{ 
                "AND(true, false)",
                "AND(true, false)",
                "IF(true, 2, 10)",
                "IF(false, 2, 10)",
                "NOT(true)",
                "OR(true, false)",
            });

            #endregion

            #region Mathematical

            categoryItem = new NTreeViewItem("Mathematical");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[]{ 
               "ABS(-2.5)",
               "CEILING(1.7)",
               "CEILING(1.7, 0.25)",
               "FLOOR(1.7)",
               "FLOOR(1.7, 0.25)",
               "INT(1.2)",
               "INT(-1.2)",
               "INTUP(1.2)",
               "INTUP(-1.2)",
               "LN(10)",
               "LOG10(10)",
               "MAGNITUDE(3, 4)",
               "MAX(1, 3, 2)",
               "MIN(1, 3, 2)",
               "MOD(5, 1.4)",
               "MOD(5, -1.4)",
               "POW(10, 2)",
               "ROUND(123.654,2)",
               "ROUND(123.654,0)",
               "ROUND(123.654,-1)",
               "SIGN(-10)",
               "SIGN(0)",
               "SQRT(4)",
               "SUM(1,2,3)",
               "TRUNC(123.654,2)",
               "TRUNC(123.654,0)",
               "TRUNC(123.654,-1)",
           });

            #endregion

            #region Text
            
            categoryItem = new NTreeViewItem("Text");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[]{ 
                "CHAR(9)",
                "LEN(\"Hello World\")",
                "LOWER(\"Hello World\")",
                "STRSAME(\"Hello\", \"hello\")",
                "STRSAME(\"Hello\", \"hello\", true)",
                "TRIM(\" Hello World \")",
                "UPPER(\"Hello World\")",
                "INDEX(0,\"Hello;World\")",
            });

            #endregion

            #region Trigonometrical

            NTreeViewItem trigonometrical = new NTreeViewItem("Trigonometrical");
            folderItem.Items.Add(trigonometrical);
            CreateTestItems(trigonometrical , new string[]{
                "ACOS(0)",
                "ANG360(1.4 + 2 * PI())",
                "ASIN(1)",
                "ATAN2(1,1)",
                "ATAN2(1,SQRT(3))",
                "ATAN(1)",
                "COS(0)",
                "COSH(PI()/4)",
                "PI()",
                "SIN(0)",
                "SINH(PI()/4)",
                "TAN(PI()/4)",
                "TANH(-PI()/4)"
            });

            #endregion

            #region Type

            NTreeViewItem type = new NTreeViewItem("Type");
            folderItem.Items.Add(type);
            CreateTestItems(type, new string[]{
               "EMPTY()",
               "ISARRAY(ARRAY(10,20))",
               "ISARRAY(10)",
               "ISBOOL(true)",
               "ISBOOL(false)",
               "ISBOOL(\"true\")",
               "ISDATETIME(10)",
               "ISDATETIME(DATETIME(2008,9,15))",
               "ISEMPTY(EMPTY())",
               "ISEMPTY(true)",
               "ISMEASURE(10[mm])",
               "ISMEASURE(10)",
               "ISNUM(10)",
               "ISNUM(true)",
               "ISSTR(true)",
               "ISSTR(\"hello world\")",
               "TOBOOL(\"false\")",
               "TOBOOL(\"true\")",
               "TOBOOL(\"hello\")",
               "TODATETIME(\"2008-09-15 09:30:41.770\")",
               "TONUM(true)",
               "TONUM(\"10\")",
               "TONUM(\"hello\")",
               "TOSTR(10)"
            });

            #endregion

            #endregion

            treeView.ExpandAll(true);
            treeView.SelectedPathChanged += new Function<NValueChangeEventArgs>(OnTestsTreeViewSelectedPathChanged);

            return treeView;
        }
        void CreateTestItems(NTreeViewItem parentItem, string[] formulas)
        {
            for (int i = 0; i < formulas.Length; i++)
            {
                parentItem.Items.Add(CreateTestItem(formulas[i]));
            }
        }
        NTreeViewItem CreateTestItem(string formula)
        {
            NTreeViewItem item = new NTreeViewItem(formula);
            item.Tag = formula;
            return item;
        }

        void OnTestsTreeViewSelectedPathChanged(NValueChangeEventArgs arg)
        {
            NTreeViewItem item = m_TestsTreeView.SelectedItem;
            if (item == null || item.Tag == null)
                return;

            m_InputTextBox.Text = item.Tag.ToString();
            EvaluateFormula();
        }
        void OnEvaluateButtonClick(NEventArgs arg)
        {
            EvaluateFormula();
        }
        void OnEvaluateAllButtonClick(NEventArgs arg)
        {
            NList<string> tests = new NList<string>();

            INIterator<NNode> it = m_TestsTreeView.GetSubtreeIterator();
            while (it.MoveNext())
            {
                NTreeViewItem item = it.Current as NTreeViewItem;
                if (item == null || item.Tag == null || !(item.Tag is string))
                    continue;

                tests.Add((string)item.Tag);
            }

            NStopwatch stopwatch = new NStopwatch();

            stopwatch.Start();
            int itcount = 10000;
            for (int j = 0; j < itcount; j++)
            {
                for (int i = 0; i < tests.Count; i++)
                {
                    try
                    {
                        m_FormulaEngine.Evaluate(tests[i]);
                    }
                    catch (Exception ex)
                    {
                        m_ResultTextBox.Text = "Failed on test: " + tests[i] + ". Error was: " + ex.Message;
                        m_InputTextBox.Text = tests[i];
                        return;
                    }
                }
            }
            stopwatch.Stop();

            int ms = stopwatch.ElapsedMilliseconds;
            m_ResultTextBox.Text = tests.Count + " tests performed " + itcount + " times in: " + ms + " milliseconds.";
        }

        void EvaluateFormula()
        {
            try
            {
                NVariant result = m_FormulaEngine.Evaluate(m_InputTextBox.Text);
                m_ResultTextBox.Text = result.ToString();
            }
            catch (Exception ex)
            {
                m_ResultTextBox.Text = ex.Message;
            }
        }

        #endregion

        #region Fields

        NTextBox m_InputTextBox;
        NTextBox m_ResultTextBox;
        NTreeView m_TestsTreeView;
        NFormulaEngine m_FormulaEngine;

        #endregion

        #region Schema

        public static readonly NSchema NFormulasExampleSchema;

        #endregion
    }
}
