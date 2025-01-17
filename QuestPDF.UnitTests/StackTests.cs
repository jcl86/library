﻿using FluentAssertions;
using NUnit.Framework;
using QuestPDF.Drawing.SpacePlan;
using QuestPDF.Elements;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.UnitTests.TestEngine;

namespace QuestPDF.UnitTests
{
    [TestFixture]
    public class StackTests
    {
        #region Measure

        [Test]
        public void Measure_ReturnsWrap_WhenFirstChildWraps()
        {
            TestPlan
                .For(x => new SimpleStack
                {
                    First = x.CreateChild("first"),
                    Second = x.CreateChild("second")
                })
                .MeasureElement(new Size(400, 300))
                .ExpectChildMeasure("first", new Size(400, 300), new Wrap())
                .CheckMeasureResult(new Wrap());
        }
        
        [Test]
        public void Measure_ReturnsPartialRender_WhenFirstChildReturnsPartialRender()
        {
            TestPlan
                .For(x => new SimpleStack
                {
                    First = x.CreateChild("first"),
                    Second = x.CreateChild("second")
                })
                .MeasureElement(new Size(400, 300))
                .ExpectChildMeasure("first", new Size(400, 300), new PartialRender(300, 200))
                .CheckMeasureResult(new PartialRender(300, 200));
        }
        
        [Test]
        public void Measure_ReturnsPartialRender_WhenSecondChildWraps()
        {
            TestPlan
                .For(x => new SimpleStack
                {
                    First = x.CreateChild("first"),
                    Second = x.CreateChild("second")
                })
                .MeasureElement(new Size(400, 300))
                .ExpectChildMeasure("first", new Size(400, 300), new FullRender(200, 100))
                .ExpectChildMeasure("second", new Size(400, 200), new Wrap())
                .CheckMeasureResult(new PartialRender(200, 100));
        }
        
        [Test]
        public void Measure_ReturnsPartialRender_WhenSecondChildReturnsPartialRender()
        {
            TestPlan
                .For(x => new SimpleStack
                {
                    First = x.CreateChild("first"),
                    Second = x.CreateChild("second")
                })
                .MeasureElement(new Size(400, 300))
                .ExpectChildMeasure("first", new Size(400, 300), new FullRender(200, 100))
                .ExpectChildMeasure("second", new Size(400, 200), new PartialRender(300, 150))
                .CheckMeasureResult(new PartialRender(300, 250));
        }
        
        [Test]
        public void Measure_ReturnsFullRender_WhenSecondChildReturnsFullRender()
        {
            TestPlan
                .For(x => new SimpleStack
                {
                    First = x.CreateChild("first"),
                    Second = x.CreateChild("second")
                })
                .MeasureElement(new Size(400, 300))
                .ExpectChildMeasure("first", new Size(400, 300), new FullRender(200, 100))
                .ExpectChildMeasure("second", new Size(400, 200), new FullRender(100, 50))
                .CheckMeasureResult(new FullRender(200, 150));
        }
        
        [Test]
        public void Measure_UsesEmpty_WhenFirstChildIsRendered()
        {
            TestPlan
                .For(x => new SimpleStack
                {
                    First = x.CreateChild("first"),
                    Second = x.CreateChild("second"),
                    
                    IsFirstRendered = true
                })
                .MeasureElement(new Size(400, 300))
                .ExpectChildMeasure("second", new Size(400, 300), new FullRender(200, 300))
                .CheckMeasureResult(new FullRender(200, 300));
        }
        
        #endregion
        
        #region Draw
        
        [Test]
        public void Draw_WhenFirstChildWraps()
        {
            TestPlan
                .For(x => new SimpleStack
                {
                    First = x.CreateChild("first"),
                    Second = x.CreateChild("second")
                })
                .DrawElement(new Size(400, 300))
                .ExpectChildMeasure("first", new Size(400, 300), new Wrap())
                .CheckDrawResult();
        }
        
        [Test]
        public void Draw_WhenFirstChildPartiallyRenders()
        {
            TestPlan
                .For(x => new SimpleStack
                {
                    First = x.CreateChild("first"),
                    Second = x.CreateChild("second")
                })
                .DrawElement(new Size(400, 300))
                .ExpectChildMeasure("first", new Size(400, 300), new PartialRender(200, 100))
                .ExpectChildDraw("first", new Size(400, 100))
                .CheckDrawResult();
        }
        
        [Test]
        public void Draw_WhenFirstChildFullyRenders_AndSecondChildWraps()
        {
            TestPlan
                .For(x => new SimpleStack
                {
                    First = x.CreateChild("first"),
                    Second = x.CreateChild("second")
                })
                .DrawElement(new Size(400, 300))
                .ExpectChildMeasure("first", new Size(400, 300), new FullRender(200, 100))
                .ExpectChildDraw("first", new Size(400, 100))
                .ExpectChildMeasure("second", new Size(400, 200), new Wrap())
                .CheckDrawResult();
        }
        
        [Test]
        public void Draw_WhenFirstChildFullyRenders_AndSecondChildPartiallyRenders()
        {
            TestPlan
                .For(x => new SimpleStack
                {
                    First = x.CreateChild("first"),
                    Second = x.CreateChild("second")
                })
                .DrawElement(new Size(400, 300))
                .ExpectChildMeasure("first", new Size(400, 300), new FullRender(200, 100))
                .ExpectChildDraw("first", new Size(400, 100))
                .ExpectChildMeasure("second", new Size(400, 200), new PartialRender(250, 150))
                .ExpectCanvasTranslate(0, 100)
                .ExpectChildDraw("second", new Size(400, 150))
                .ExpectCanvasTranslate(0, -100)
                .CheckDrawResult();
        }
        
        [Test]
        public void Draw_WhenFirstChildFullyRenders_AndSecondChildFullyRenders()
        {
            TestPlan
                .For(x => new SimpleStack
                {
                    First = x.CreateChild("first"),
                    Second = x.CreateChild("second")
                })
                .DrawElement(new Size(400, 300))
                .ExpectChildMeasure("first", new Size(400, 300), new FullRender(200, 100))
                .ExpectChildDraw("first", new Size(400, 100))
                .ExpectChildMeasure("second", new Size(400, 200), new FullRender(250, 150))
                .ExpectCanvasTranslate(0, 100)
                .ExpectChildDraw("second", new Size(400, 150))
                .ExpectCanvasTranslate(0, -100)
                .CheckDrawResult();
        }
        
        [Test]
        public void Draw_UsesEmpty_WhenFirstChildIsRendered()
        {
            TestPlan
                .For(x => new SimpleStack
                {
                    First = x.CreateChild("first"),
                    Second = x.CreateChild("second"),
                    
                    IsFirstRendered = true
                })
                .DrawElement(new Size(400, 300))
                .ExpectChildMeasure("second", new Size(400, 300), new PartialRender(200, 300))
                .ExpectCanvasTranslate(0, 0)
                .ExpectChildDraw("second", new Size(400, 300))
                .ExpectCanvasTranslate(0, 0)
                .CheckState<SimpleStack>(x => x.IsFirstRendered)
                .CheckDrawResult();
        }
        
        [Test]
        public void Draw_TogglesFirstRenderedFlag_WhenSecondFullyRenders()
        {
            TestPlan
                .For(x => new SimpleStack
                {
                    First = x.CreateChild("first"),
                    Second = x.CreateChild("second"),
                    
                    IsFirstRendered = true
                })
                .DrawElement(new Size(400, 300))
                .ExpectChildMeasure("second", new Size(400, 300), new FullRender(200, 300))
                .ExpectCanvasTranslate(0, 0)
                .ExpectChildDraw("second", new Size(400, 300))
                .ExpectCanvasTranslate(0, 0)
                .CheckDrawResult()
                .CheckState<SimpleStack>(x => !x.IsFirstRendered);
        }
        
        #endregion
        
        #region Structure
        
        [Test]
        public void Structure_Simple()
        { 
            // arrange
            var childA = TestPlan.CreateUniqueElement();
            var childB = TestPlan.CreateUniqueElement();
            var childC = TestPlan.CreateUniqueElement();
            var childD = TestPlan.CreateUniqueElement();
            var childE = TestPlan.CreateUniqueElement();

            const int spacing = 20;
            
            // act
            var structure = new Container();
            
            structure.Stack(stack =>
            {
                stack.Spacing(spacing);
                
                stack.Item().Element(childA);
                stack.Item().Element(childB);
                stack.Item().Element(childC);
                stack.Item().Element(childD);
                stack.Item().Element(childE);
            });
            
            // assert
            var expected = new Padding
            {
                Bottom = -spacing,

                Child = new SimpleStack
                {
                    First = new SimpleStack
                    {
                        First = new Padding
                        {
                            Bottom = spacing,
                            Child = childA
                        },
                        Second = new Padding
                        {
                            Bottom = spacing,
                            Child = childB
                        }
                    },
                    Second = new SimpleStack
                    {
                        First = new Padding
                        {
                            Bottom = spacing,
                            Child = childC
                        },
                        Second = new SimpleStack
                        {
                            First = new Padding
                            {
                                Bottom = spacing,
                                Child = childD
                            },
                            Second = new Padding
                            {
                                Bottom = spacing,
                                Child = childE
                            }
                        }
                    }
                }
            };

            TestPlan.CompareOperations(structure, expected);
        }
        
        #endregion
    }
}