using System.Collections.Generic;

namespace rels.Model
{
    public static class Init
    {

        public static readonly List<string> CREATE_SQL = new List<string>() {
            @"CREATE TABLE IF NOT EXISTS 'People' (
	            'ID'    INTEGER NOT NULL UNIQUE,
	            'WikiDataID'    TEXT NOT NULL UNIQUE,
	            'Name'  TEXT NOT NULL,
	            'RusName'   TEXT,
                'ImageFile'	TEXT,
				'Country'   TEXT,
	            'DateOfBirth'   TEXT,
	            'DateOfDeath'   TEXT,
	            'Father'    TEXT,
	            'Mother'    TEXT,
	         PRIMARY KEY('ID')
            );",

            @"CREATE TABLE IF NOT EXISTS 'Countries' (
	          'ID'    INTEGER NOT NULL UNIQUE,
	          'WikiDataID'    TEXT NOT NULL UNIQUE,
	          'Name'  TEXT NOT NULL,
	          'RusName'   TEXT,
	          PRIMARY KEY('ID')
            )",
        };

    }
}
