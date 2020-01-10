using System;
using System.Globalization;
using System.Text;

namespace MrHuo.Extensions
{
    internal static class GeneratedNames
    {
        internal const string SynthesizedLocalNamePrefix = "CS$";
        internal const char DotReplacementInTypeNames = '-';
        private const string SuffixSeparator = "__";
        private const char IdSeparator = '_';
        private const char GenerationSeparator = '#';
        private const char LocalFunctionNameTerminator = '|';
        internal const string AnonymousNamePrefix = "<>f__AnonymousType";

        internal static bool IsGeneratedMemberName(string memberName)
        {
            if (memberName.Length > 0)
                return memberName[0] == '<';
            return false;
        }

        internal static string MakeBackingFieldName(string propertyName)
        {
            return "<" + propertyName + ">k__BackingField";
        }

        internal static string MakeIteratorFinallyMethodName(int iteratorState)
        {
            return "<>m__Finally" + StringExtensions.GetNumeral(Math.Abs(iteratorState + 2));
        }

        internal static string MakeStaticLambdaDisplayClassName(int methodOrdinal, int generation)
        {
            return GeneratedNames.MakeMethodScopedSynthesizedName(GeneratedNameKind.LambdaDisplayClass, methodOrdinal, generation, null, null, char.MinValue, -1, -1);
        }

        internal static string MakeLambdaDisplayClassName(
          int methodOrdinal,
          int generation,
          int closureOrdinal,
          int closureGeneration)
        {
            return GeneratedNames.MakeMethodScopedSynthesizedName(GeneratedNameKind.LambdaDisplayClass, methodOrdinal, generation, null, "DisplayClass", char.MinValue, closureOrdinal, closureGeneration);
        }

        internal static string MakeAnonymousTypeTemplateName(
          int index,
          int submissionSlotIndex,
          string moduleId)
        {
            string str = "<" + moduleId + ">f__AnonymousType" + StringExtensions.GetNumeral(index);
            if (submissionSlotIndex >= 0)
                str = str + "#" + StringExtensions.GetNumeral(submissionSlotIndex);
            return str;
        }

        internal static bool TryParseAnonymousTypeTemplateName(string name, out int index)
        {
            if (name.StartsWith("<>f__AnonymousType", StringComparison.Ordinal) && int.TryParse(name.Substring("<>f__AnonymousType".Length), NumberStyles.None, CultureInfo.InvariantCulture, out index))
                return true;
            index = -1;
            return false;
        }

        internal static string MakeAnonymousTypeBackingFieldName(string propertyName)
        {
            return "<" + propertyName + ">i__Field";
        }

        internal static string MakeAnonymousTypeParameterName(string propertyName)
        {
            return "<" + propertyName + ">j__TPar";
        }

        internal static bool TryParseAnonymousTypeParameterName(
          string typeParameterName,
          out string propertyName)
        {
            if (typeParameterName.StartsWith("<", StringComparison.Ordinal) && typeParameterName.EndsWith(">j__TPar", StringComparison.Ordinal))
            {
                propertyName = typeParameterName.Substring(1, typeParameterName.Length - 9);
                return true;
            }
            propertyName = null;
            return false;
        }

        internal static string MakeStateMachineTypeName(
          string methodName,
          int methodOrdinal,
          int generation)
        {
            return GeneratedNames.MakeMethodScopedSynthesizedName(GeneratedNameKind.StateMachineType, methodOrdinal, generation, methodName, null, char.MinValue, -1, -1);
        }

        internal static string MakeBaseMethodWrapperName(int uniqueId)
        {
            return "<>n__" + StringExtensions.GetNumeral(uniqueId);
        }

        internal static string MakeLambdaMethodName(
          string methodName,
          int methodOrdinal,
          int methodGeneration,
          int lambdaOrdinal,
          int lambdaGeneration)
        {
            return GeneratedNames.MakeMethodScopedSynthesizedName(GeneratedNameKind.LambdaMethod, methodOrdinal, methodGeneration, methodName, null, char.MinValue, lambdaOrdinal, lambdaGeneration);
        }

        internal static string MakeLambdaCacheFieldName(
          int methodOrdinal,
          int generation,
          int lambdaOrdinal,
          int lambdaGeneration)
        {
            return GeneratedNames.MakeMethodScopedSynthesizedName(GeneratedNameKind.LambdaCacheField, methodOrdinal, generation, null, null, char.MinValue, lambdaOrdinal, lambdaGeneration);
        }

        internal static string MakeLocalFunctionName(
          string methodName,
          string localFunctionName,
          int methodOrdinal,
          int methodGeneration,
          int lambdaOrdinal,
          int lambdaGeneration)
        {
            return GeneratedNames.MakeMethodScopedSynthesizedName(GeneratedNameKind.LocalFunction, methodOrdinal, methodGeneration, methodName, localFunctionName, '|', lambdaOrdinal, lambdaGeneration);
        }

        private static string MakeMethodScopedSynthesizedName(
          GeneratedNameKind kind,
          int methodOrdinal,
          int methodGeneration,
          string methodNameOpt = null,
          string suffix = null,
          char suffixTerminator = '\0',
          int entityOrdinal = -1,
          int entityGeneration = -1)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('<');
            if (methodNameOpt != null)
            {
                builder.Append(methodNameOpt);
                if (kind.IsTypeName())
                    builder.Replace('.', '-');
            }
            builder.Append('>');
            builder.Append((char)kind);
            if (suffix != null || methodOrdinal >= 0 || entityOrdinal >= 0)
            {
                builder.Append("__");
                builder.Append(suffix);
                if (suffixTerminator != char.MinValue)
                    builder.Append(suffixTerminator);
                if (methodOrdinal >= 0)
                {
                    builder.Append(methodOrdinal);
                    GeneratedNames.AppendOptionalGeneration(builder, methodGeneration);
                }
                if (entityOrdinal >= 0)
                {
                    if (methodOrdinal >= 0)
                        builder.Append('_');
                    builder.Append(entityOrdinal);
                    GeneratedNames.AppendOptionalGeneration(builder, entityGeneration);
                }
            }
            return builder.ToString();
        }

        private static void AppendOptionalGeneration(StringBuilder builder, int generation)
        {
            if (generation <= 0)
                return;
            builder.Append('#');
            builder.Append(generation);
        }

        internal static GeneratedNameKind GetKind(string name)
        {
            GeneratedNameKind kind;
            int openBracketOffset;
            int closeBracketOffset;
            if (!GeneratedNames.TryParseGeneratedName(name, out kind, out openBracketOffset, out closeBracketOffset))
                return GeneratedNameKind.None;
            return kind;
        }

        internal static bool TryParseGeneratedName(
          string name,
          out GeneratedNameKind kind,
          out int openBracketOffset,
          out int closeBracketOffset)
        {
            openBracketOffset = -1;
            if (name.StartsWith("CS$<", StringComparison.Ordinal))
                openBracketOffset = 3;
            else if (name.StartsWith("<", StringComparison.Ordinal))
                openBracketOffset = 0;
            if (openBracketOffset >= 0)
            {
                closeBracketOffset = name.IndexOfBalancedParenthesis(openBracketOffset, '>');
                if (closeBracketOffset >= 0 && closeBracketOffset + 1 < name.Length)
                {
                    int num = name[closeBracketOffset + 1];
                    if (num >= 49 && num <= 57 || num >= 97 && num <= 122)
                    {
                        kind = (GeneratedNameKind)num;
                        return true;
                    }
                }
            }
            kind = GeneratedNameKind.None;
            openBracketOffset = -1;
            closeBracketOffset = -1;
            return false;
        }

        internal static bool TryParseSourceMethodNameFromGeneratedName(
          string generatedName,
          GeneratedNameKind requiredKind,
          out string methodName)
        {
            GeneratedNameKind kind;
            int openBracketOffset;
            int closeBracketOffset;
            if (!GeneratedNames.TryParseGeneratedName(generatedName, out kind, out openBracketOffset, out closeBracketOffset))
            {
                methodName = null;
                return false;
            }
            if (requiredKind != GeneratedNameKind.None && kind != requiredKind)
            {
                methodName = null;
                return false;
            }
            methodName = generatedName.Substring(openBracketOffset + 1, closeBracketOffset - openBracketOffset - 1);
            if (kind.IsTypeName())
                methodName = methodName.Replace('-', '.');
            return true;
        }

        internal static string AsyncAwaiterFieldName(int slotIndex)
        {
            return "<>u__" + StringExtensions.GetNumeral(slotIndex + 1);
        }

        internal static bool TryParseSlotIndex(string fieldName, out int slotIndex)
        {
            int num = fieldName.LastIndexOf('_');
            if (num - 1 < 0 || num == fieldName.Length || fieldName[num - 1] != '_')
            {
                slotIndex = -1;
                return false;
            }
            if (int.TryParse(fieldName.Substring(num + 1), NumberStyles.None, CultureInfo.InvariantCulture, out slotIndex) && slotIndex >= 1)
            {
                --slotIndex;
                return true;
            }
            slotIndex = -1;
            return false;
        }

        internal static string MakeCachedFrameInstanceFieldName()
        {
            return "<>9";
        }

        internal static string MakeLambdaDisplayLocalName(int uniqueId)
        {
            return "CS$<>8__locals" + StringExtensions.GetNumeral(uniqueId);
        }

        internal static bool IsSynthesizedLocalName(string name)
        {
            return name.StartsWith("CS$", StringComparison.Ordinal);
        }

        internal static string MakeFixedFieldImplementationName(string fieldName)
        {
            return "<" + fieldName + ">e__FixedBuffer";
        }

        internal static string MakeStateMachineStateFieldName()
        {
            return "<>1__state";
        }

        internal static string MakeIteratorCurrentFieldName()
        {
            return "<>2__current";
        }

        internal static string MakeIteratorCurrentThreadIdFieldName()
        {
            return "<>l__initialThreadId";
        }

        internal static string ThisProxyFieldName()
        {
            return "<>4__this";
        }

        internal static string StateMachineThisParameterProxyName()
        {
            return GeneratedNames.StateMachineParameterProxyFieldName(GeneratedNames.ThisProxyFieldName());
        }

        internal static string StateMachineParameterProxyFieldName(string parameterName)
        {
            return "<>3__" + parameterName;
        }

        internal static string MakeDynamicCallSiteContainerName(int methodOrdinal, int generation)
        {
            return GeneratedNames.MakeMethodScopedSynthesizedName(GeneratedNameKind.DynamicCallSiteContainerType, methodOrdinal, generation, null, null, char.MinValue, -1, -1);
        }

        internal static string MakeDynamicCallSiteFieldName(int uniqueId)
        {
            return "<>p__" + StringExtensions.GetNumeral(uniqueId);
        }

        internal static string AsyncBuilderFieldName()
        {
            return "<>t__builder";
        }

        internal static string ReusableHoistedLocalFieldName(int number)
        {
            return "<>7__wrap" + StringExtensions.GetNumeral(number);
        }

        internal static string LambdaCopyParameterName(int ordinal)
        {
            return "<p" + StringExtensions.GetNumeral(ordinal) + ">";
        }
    }
}
