﻿using NpgsqlTypes;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace ECommerce.WebAPI.Configurations.ColumnWriters
{
    public class UserNameColumnWriter:ColumnWriterBase
    {
        public UserNameColumnWriter() : base(NpgsqlDbType.Varchar,columnLength:50)
        {
        }

        public override object? GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
        {
            var (username, value) = logEvent.Properties.FirstOrDefault(p=>p.Key=="user_name");
            return value?.ToString() ?? null;
        }
    }
}
