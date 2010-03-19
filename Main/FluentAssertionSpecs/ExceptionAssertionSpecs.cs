﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace FluentAssertions.specs
{
    [TestClass]
    public class ExceptionAssertionSpecs
    {
        [TestMethod]
        public void When_subject_throws_expected_exception_with_an_expected_message_it_should_not_do_anything()
        {
            IFoo testSubject = MockRepository.GenerateStub<IFoo>();
            testSubject.Stub(x => x.Do()).Throw(new InvalidOperationException("some message"));

            testSubject.ShouldThrow(x => x.Do()).Exception<InvalidOperationException>().WithMessage("some message");
        }

        [TestMethod]
        [ExpectedException(typeof(SpecificationMismatchException))]
        public void When_subject_throws_expected_exception_but_with_unexpected_message_it_should_throw()
        {
            IFoo testSubject = MockRepository.GenerateStub<IFoo>();
            testSubject.Stub(x => x.Do()).Throw(new InvalidOperationException("unexpected message"));

            testSubject.ShouldThrow(x => x.Do()).Exception<InvalidOperationException>().WithMessage("expected message");
        }

        [TestMethod]
        public void When_subject_throws_some_exception_with_unexpected_message_it_should_throw_with_clear_description()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo subjectThatThrows = MockRepository.GenerateStub<IFoo>();
            subjectThatThrows.Stub(x => x.Do()).Throw(new InvalidOperationException("message1"));

            try
            {
                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                subjectThatThrows
                    .ShouldThrow(x => x.Do())
                    .Exception<InvalidOperationException>()
                    .WithMessage("message2");

                Assert.Fail("This point should not be reached");
            }
            catch (SpecificationMismatchException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                Assert.AreEqual(
                    "Expected exception with message \"message2\", but \"message1\" differs near '1' (index 7).",
                    ex.Message);
            }
        }

        [TestMethod]
        public void
            When_subject_does_not_throw_exception_but_one_was_expected_with_reason_it_should_throw_with_clear_description_and_reason
            ()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();

                testSubject.ShouldThrow(x => x.Do()).Exception<Exception>("because {0} should do that", "IFoo.Do");

                Assert.Fail("This point should not be reached");
            }
            catch (SpecificationMismatchException ex)
            {
                ex.Message.Should().Be(
                    "Expected <System.Exception> because IFoo.Do should do that, but no exception was thrown.");
            }
        }

        [TestMethod]
        public void When_subject_throws_another_exception_than_expected_it_should_throw_with_clear_description()
        {
            IFoo testSubject = MockRepository.GenerateStub<IFoo>();
            testSubject.Stub(x => x.Do()).Throw(new ArgumentException());

            try
            {
                testSubject
                    .ShouldThrow(x => x.Do())
                    .Exception<InvalidOperationException>();

                Assert.Fail("This point should not be reached");
            }
            catch (SpecificationMismatchException ex)
            {
                ex.Message.Should().Be(
                    "Expected <System.InvalidOperationException>, but found <System.ArgumentException>.");
            }
        }

        [TestMethod]
        public void
            When_subject_throws_another_exception_than_expected_it_should_throw_with_clear_description_and_reason()
        {
            IFoo testSubject = MockRepository.GenerateStub<IFoo>();
            testSubject.Stub(x => x.Do()).Throw(new ArgumentException());

            try
            {
                testSubject
                    .ShouldThrow(x => x.Do())
                    .Exception<InvalidOperationException>("because {0} should throw that one", "IFoo.Do");

                Assert.Fail("This point should not be reached");
            }
            catch (SpecificationMismatchException ex)
            {
                ex.Message.Should().Be(
                    "Expected <System.InvalidOperationException> because IFoo.Do should throw that one, but found <System.ArgumentException>.");
            }
        }

        [TestMethod]
        public void When_subject_throws_an_exception_with_the_expected_innerexception_it_should_not_do_anything()
        {
            IFoo testSubject = MockRepository.GenerateStub<IFoo>();
            testSubject.Stub(x => x.Do()).Throw(new Exception("", new ArgumentException()));

            testSubject.ShouldThrow(x => x.Do()).Exception<Exception>()
                .WithInnerException<ArgumentException>();
        }

        [TestMethod]
        public void
            When_subject_throws_an_exception_with_an_unexpected_innerexception_it_should_throw_with_clear_description()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();
                testSubject.Stub(x => x.Do()).Throw(new Exception("", new NullReferenceException()));

                testSubject.ShouldThrow(x => x.Do()).Exception<Exception>()
                    .WithInnerException<ArgumentException>();

                Assert.Fail("This point should not be reached");
            }
            catch (SpecificationMismatchException ex)
            {
                ex.Message.Should().Be(
                    "Expected inner <System.ArgumentException>, but found <System.NullReferenceException>.");
            }
        }

        [TestMethod]
        public void
            When_subject_throws_an_exception_with_an_unexpected_innerexception_it_should_throw_with_clear_description_reason
            ()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();
                testSubject.Stub(x => x.Do()).Throw(new Exception("", new NullReferenceException()));

                testSubject.ShouldThrow(x => x.Do()).Exception<Exception>()
                    .WithInnerException<ArgumentException>("because {0} should do just that", "IFoo.Do");

                Assert.Fail("This point should not be reached");
            }
            catch (SpecificationMismatchException ex)
            {
                ex.Message.Should().Be(
                    "Expected inner <System.ArgumentException> because IFoo.Do should do just that, but found <System.NullReferenceException>.");
            }
        }

        [TestMethod]
        public void
            When_subject_throws_an_exception_without_expected_inner_exception_it_should_throw_with_clear_description()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();
                testSubject.Stub(x => x.Do()).Throw(new Exception(""));

                testSubject.ShouldThrow(x => x.Do()).Exception<Exception>()
                    .WithInnerException<InvalidOperationException>();

                Assert.Fail("This point should not be reached");
            }
            catch (SpecificationMismatchException ex)
            {
                ex.Message.Should().Be(
                    "Expected inner <System.InvalidOperationException>, but the thrown exception has no inner exception.");
            }
        }

        [TestMethod]
        public void
            When_subject_throws_an_exception_without_expected_inner_exception_and_has_reason_it_should_throw_with_clear_description
            ()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();
                testSubject.Stub(x => x.Do()).Throw(new Exception(""));

                testSubject.ShouldThrow(x => x.Do()).Exception<Exception>()
                    .WithInnerException<InvalidOperationException>("because {0} should do that", "IFoo.Do");

                Assert.Fail("This point should not be reached");
            }
            catch (SpecificationMismatchException ex)
            {
                ex.Message.Should().Be(
                    "Expected inner <System.InvalidOperationException> because IFoo.Do should do that, but the thrown exception has no inner exception.");
            }
        }

        [TestMethod]
        public void When_subject_throws_inner_exception_with_expected_message_it_should_not_do_anything()
        {
            IFoo testSubject = MockRepository.GenerateStub<IFoo>();
            testSubject.Stub(x => x.Do()).Throw(new Exception("", new InvalidOperationException("expected message")));

            testSubject.ShouldThrow(x => x.Do()).Exception<Exception>()
                .WithInnerMessage("expected message");
        }

        [TestMethod]
        public void When_subject_throws_inner_exception_with_unexpected_message_it_should_throw_with_clear_description()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();
                testSubject.Stub(x => x.Do()).Throw(new Exception("", new InvalidOperationException("unexpected message")));

                testSubject.ShouldThrow(x => x.Do()).Exception<Exception>()
                    .WithInnerMessage("expected message");

                Assert.Fail("This point should not be reached");
            }
            catch (SpecificationMismatchException ex)
            {
                ex.Message.Should().Be(
                    "Expected inner exception with message \"expected message\", but \"unexpected message\" differs near 'u' (index 0).");
            }
        }

        [TestMethod]
        public void When_subject_throws_inner_exception_with_unexpected_message_it_should_throw_with_clear_description_and_reason()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();
                testSubject.Stub(x => x.Do()).Throw(new Exception("", new InvalidOperationException("unexpected message")));

                testSubject.ShouldThrow(x => x.Do()).Exception<Exception>()
                    .WithInnerMessage("expected message", "because {0} should do just that", "IFoo.Do");

                Assert.Fail("This point should not be reached");
            }
            catch (SpecificationMismatchException ex)
            {
                ex.Message.Should().Be(
                    "Expected inner exception with message \"expected message\" because IFoo.Do should do just that, but \"unexpected message\" differs near 'u' (index 0).");
            }
        }

        [TestMethod]
        public void When_getting_value_of_property_of_thrown_exception_it_should_return_value_of_property()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------            
            const string SomeParamNameValue = "param";
            var target = MockRepository.GenerateStub<IFoo>();
            target.Stub(t => t.Do()).Throw(new ArgumentException("message", SomeParamNameValue));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = target.Do;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------            
            act.ShouldThrow()
                .Exception<ArgumentException>()
                .And.ParamName.Should().Be(SomeParamNameValue);
        }

        [TestMethod]
        public void When_validating_a_subject_against_multiple_conditions_it_should_support_chaining()
        {
            IFoo testSubject = MockRepository.GenerateStub<IFoo>();
            testSubject.Stub(x => x.Do()).Throw(
                new InvalidOperationException("message", new ArgumentException("inner message")));

            testSubject
                .ShouldThrow(x => x.Do())
                .Exception<InvalidOperationException>()
                .WithInnerMessage("inner message")
                .WithInnerException<ArgumentException>()
                .WithInnerMessage("inner message");
        }
    }

    public class SomeTestClass
    {
        internal const string ExceptionMessage = "someMessage";

        public IList<string> Strings = new List<string>();

        public void Throw()
        {
            throw new ArgumentException(ExceptionMessage);
        }
    }

    public interface IFoo
    {
        void Do();
    }
}