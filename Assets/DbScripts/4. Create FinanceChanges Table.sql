CREATE TABLE "FinanceChanges" (
	"ID"	INTEGER NOT NULL DEFAULT 1,
	"DateOfFixation"	TEXT NOT NULL,
	"Summ"	NUMERIC NOT NULL DEFAULT 0.0,
	"Currency"	INTEGER NOT NULL DEFAULT 0,
	"Comment"	TEXT,
	"FixedBy"	INTEGER NOT NULL DEFAULT 0,
	"Category"	INTEGER NOT NULL DEFAULT 0,
	"SumInIternationalCurrency"	NUMERIC NOT NULL DEFAULT 0,
	FOREIGN KEY("Currency") REFERENCES "Currencies"("ID") ON DELETE SET DEFAULT,
	FOREIGN KEY("FixedBy") REFERENCES "Users"("ID") ON DELETE SET DEFAULT,
	FOREIGN KEY("Category") REFERENCES "Categories"("ID") ON DELETE SET DEFAULT,
	PRIMARY KEY("ID" AUTOINCREMENT)
);