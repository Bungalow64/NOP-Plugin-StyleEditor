using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Nop.Plugin.Admin.StyleEditor.Helpers;
using Nop.Plugin.Admin.StyleEditor.Settings;
using NUnit.Framework;

namespace Nop.Plugin.Admin.StyleEditor.Tests.Settings
{
    [TestFixture]
    public class StyleEditorSettingsTests
    {
        [Test]
        public void UpdateVersion_NullHelper_ThrowException()
        {
            var settings = new StyleEditorSettings();

            Assert.Throws<ArgumentNullException>(() => settings.UpdateVersion(null));
        }

        [Test]
        public void UpdateVersion_CalledMultipleTimes_DifferentVersions()
        {
            var date1 = DateTime.Parse("02-Mar-2021 09:00:00");
            var date2 = DateTime.Parse("02-Mar-2021 09:00:01");

            var currentDateTimeHelper = new Mock<ICurrentDateTimeHelper>(MockBehavior.Strict);
            currentDateTimeHelper
                .SetupSequence(p => p.UtcNow)
                .Returns(date1)
                .Returns(date2);

            var settings = new StyleEditorSettings
            {
                CustomStyles = "h1{color:red;}"
            };

            settings.UpdateVersion(currentDateTimeHelper.Object);
            var version1 = settings.Version;

            settings.UpdateVersion(currentDateTimeHelper.Object);
            var version2 = settings.Version;

            Assert.AreNotEqual(version1, version2);
        }

        [Test]
        public void CustomStylesPath_CorrectPath()
        {
            var date1 = DateTime.Parse("02-Mar-2021 09:00:00");

            var currentDateTimeHelper = new Mock<ICurrentDateTimeHelper>(MockBehavior.Strict);
            currentDateTimeHelper
                .SetupGet(p => p.UtcNow)
                .Returns(date1);

            var settings = new StyleEditorSettings
            {
                CustomStyles = "h1{color:red;}"
            };

            settings.UpdateVersion(currentDateTimeHelper.Object);
            var version1 = settings.Version;

            Assert.AreEqual("/CustomStyle?v=637502724000000000", settings.CustomStylesPath);
        }

        [Test]
        public void GenerateView_IsDisabled_ReturnNull()
        {
            var settings = new StyleEditorSettings
            {
                DisableCustomStyles = true,
                CustomStyles = "h1{color:red;}",
                UseAsync = false,
                RenderType = 1
            };

            var (view, model) = settings.GenerateView();

            Assert.IsNull(view);
            Assert.IsNull(model);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        [TestCase("  \n\r  ")]
        public void GenerateView_HasEmptyStyles_ReturnNull(string customStyles)
        {
            var settings = new StyleEditorSettings
            {
                DisableCustomStyles = false,
                CustomStyles = customStyles,
                UseAsync = false,
                RenderType = 1
            };

            var (view, model) = settings.GenerateView();

            Assert.IsNull(view);
            Assert.IsNull(model);
        }

        [Test]
        [TestCase(1)]
        [TestCase(0)]
        public void GenerateView_DisplayInline(int renderType)
        {
            var settings = new StyleEditorSettings
            {
                DisableCustomStyles = false,
                CustomStyles = "h1{color:red;}",
                UseAsync = false,
                RenderType = renderType
            };

            var (view, model) = settings.GenerateView();

            Assert.AreEqual("~/Plugins/Admin.StyleEditor/Views/CustomStyles.cshtml", view);
            Assert.AreEqual("h1{color:red;}", model);
        }

        [Test]
        public void GenerateView_DisplayAsFile_NotAsync()
        {
            var settings = new StyleEditorSettings
            {
                DisableCustomStyles = false,
                CustomStyles = "h1{color:red;}",
                UseAsync = false,
                RenderType = 2,
                Version = "100001"
            };

            var (view, model) = settings.GenerateView();

            Assert.AreEqual("~/Plugins/Admin.StyleEditor/Views/CustomStylesLink.cshtml", view);
            Assert.AreEqual($"/CustomStyle?v=100001", model);
        }

        [Test]
        public void GenerateView_DisplayAsFile_Async()
        {
            var settings = new StyleEditorSettings
            {
                DisableCustomStyles = false,
                CustomStyles = "h1{color:red;}",
                UseAsync = true,
                RenderType = 2,
                Version = "100001"
            };

            var (view, model) = settings.GenerateView();

            Assert.AreEqual("~/Plugins/Admin.StyleEditor/Views/CustomStylesAsync.cshtml", view);
            Assert.AreEqual($"/CustomStyle?v=100001", model);
        }
    }
}
