using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Nop.Plugin.Admin.StyleEditor.Settings;
using Nop.Web.Controllers;
using NUnit.Framework;

namespace Nop.Plugin.Admin.StyleEditor.Tests.Controllers
{
    [TestFixture]
    public class CustomStyleControllerTests
    {
        private Mock<StyleEditorSettings> _settings;

        [SetUp]
        public void Setup()
        {
            _settings = new Mock<StyleEditorSettings>(MockBehavior.Strict);
            _settings
                .SetupGet(p => p.DisableCustomStyles)
                .Returns(false);
            _settings
                .SetupGet(p => p.CustomStyles)
                .Returns("h1{color:red;}");
        }

        public CustomStyleController Create()
        {
            return new CustomStyleController(_settings.Object);
        }

        [Test]
        public void Index_IsDisabled_ReturnNothing()
        {
            _settings
                .SetupGet(p => p.DisableCustomStyles)
                .Returns(true);

            var result = Create().Index();

            ClassicAssert.NotNull(result);
            ClassicAssert.IsInstanceOf<ContentResult>(result);
            var castResult = (ContentResult)result;

            ClassicAssert.AreEqual(string.Empty, castResult.Content);
            ClassicAssert.AreEqual("text/css", castResult.ContentType);
        }

        [Test]
        public void Index_ReturnStyles()
        {
            var result = Create().Index();

            ClassicAssert.NotNull(result);
            ClassicAssert.IsInstanceOf<ContentResult>(result);
            var castResult = (ContentResult)result;

            ClassicAssert.AreEqual("h1{color:red;}", castResult.Content);
            ClassicAssert.AreEqual("text/css", castResult.ContentType);
        }
    }
}
