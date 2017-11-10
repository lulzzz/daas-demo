using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaaSDemo.Models.Sql
{
    /// <summary>
    ///     Request body when executing a T-SQL command (i.e. a non-query).
    /// </summary>
    public class Command
        : SqlRequest
    {
    }
}
