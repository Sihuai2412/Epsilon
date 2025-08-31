using System;
using System.CodeDom.Compiler;

namespace epsilon.Generators;

internal class CurlyIndenter : IDisposable {
    private IndentedTextWriter _indentedTextWriter;

    public CurlyIndenter(IndentedTextWriter indentedTextWriter, string openingLine = "") {
        _indentedTextWriter = indentedTextWriter;

        if (!string.IsNullOrWhiteSpace(openingLine)) {
            indentedTextWriter.Write($"{openingLine} ");
        }

        indentedTextWriter.WriteLine("{");
        indentedTextWriter.Indent++;
    }

    public void Dispose() {
        _indentedTextWriter.Indent--;
        _indentedTextWriter.WriteLine("}");
    }
}