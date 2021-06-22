This is a test project for vacancy.


Task description
---------------------
Create a program that collects and exports OData service data. Program name is declared-persons-analyser.exe which allows you to output reports based on incoming parameters.


Input data
---------------------
Name			Type		Description
----------------------------------------
source			string		Service address.
district		int			Identifier of the district. This parameter is required.
year			int			Year for which to process data.
month			int			Month for which to process data.
day				int			Day of month for which to process data.
limit			int			Limit how many records to output. If no parameter is specified, the default value is 100.
group			string		Display by groups summing the number of persons declared.
							Possible values: y (group by years), m (group by months), d (group by days).
							Examples: y, m, d, ym, yd, md.
out				string		The name of the file in which to export the output data in JSON format.


Output data
---------------------
o district_name - district title.
o year - year, if in the group specified parameter y.
o month - month, if in the group specified parameter m.
o day - day of month, if in the group specified parameter d.
o value - the sum of the value in the group.
o change - change in the value of the group against the value of the previous group.
o Max - maximum 'value' value.
o Min - minimum 'value' value.
o Average - average 'value' value, rounded to the nearest whole number.
o Max drop - maximum drop 'value', name of district and group.
o Max increase - maximum increment 'value', name of district and group.

The output data must be arranged in ascending order by group: year, month, day.

Example
---------------------
declared-persons-analyser.exe -district 516 -year 2019 -limit 4 -group ym –out res.json


Districts list
---------------------
516		Rīga
568		Daugavpils
579		Ventspils
518		Liepāja
765		Valmieras novads
50		Krustpils novads
156		Kokneses novads

Full list available in Web API project.


*******************************************
Implementation
*******************************************

Console application:
- .NET Framework 4.8
- Visual Studio 2019
- Unit tests (MSTest)

Additional implementation  (out of test requirements):
- Web API
- NSwag


