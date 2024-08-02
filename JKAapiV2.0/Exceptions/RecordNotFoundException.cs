using System;
using System.Collections.Generic;
using System.Text;

class RecordNotFoundException : Exception
{
    private string _message;

    public override string Message => _message;

    public RecordNotFoundException(string table, string id)
    {
        _message = "Could not find " + table + " with id: " + id;
    }

    public RecordNotFoundException()
    {
        _message = "Unsuccessful insertion";
    }
}