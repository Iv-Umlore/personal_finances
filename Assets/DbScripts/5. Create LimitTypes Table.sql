CREATE TABLE "LimitTypes" (
	"ID"	INTEGER NOT NULL DEFAULT 1,
	"Name"	TEXT NOT NULL,
	"Period"	TEXT NOT NULL,
	"StartPeriod"	TEXT NOT NULL,
	"IsAutoProlongation"	INTEGER NOT NULL DEFAULT 0,
	"IsActive"	INTEGER NOT NULL DEFAULT 1,
	PRIMARY KEY("ID" AUTOINCREMENT)
);