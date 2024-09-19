Find what you need faster … Home is your new landing page and surfaces your most relevant files and folders
RPA_CODES_VB.txt
'---- Read from csv using db----
Connectionstring =
"Provider=Microsoft.Jet.OLEDB.4.0; Data Source="+in_processingFolderPath+"; Extended Properties=""text;HDR=NO;FORMAT=Delimited"""

Sample Query

"Select [F12] from ["+in_CompilationFileNameWithExtension+"] where  [F12] LIKE '%VNM%' OR [F12] LIKE '%PSM%' OR [F12] LIKE '%NXG%' OR [F12] LIKE '%TAMD%' OR [F12] LIKE '%X00%' OR [F12] LIKE '%SL00%'"



'-----Add data column with a specific type-----

'add data column called narration
Trsf_DT.Columns.Add("NARRATION",GetType(String))


'---Join data tables and pick only matching elements in table 1---

Dim ex As Exception
Try
dtjoin  = dtSpoolDT.AsEnumerable().Where(Function(row)  dtCompilationDT.AsEnumerable().Any(Function(x) x("NARRATION").ToString=row("NARRATION").ToString)).CopyToDatatable
       errorMessage = ex.Message
   Catch ex
       errorMessage = ex.Message
   End Try



'---Join data tables and pick only Non matching elements in table 1---


Dim ex As Exception
Try
dtLeftOver  = dtSpoolDT.AsEnumerable().Where(Function(row) Not  dtCompilationDT.AsEnumerable().Any(Function(x) x( "NARRATION").ToString=row("NARRATION").ToString)).CopyToDatatable
   Catch ex
       errorMessage = ex.Message
   End Try

'----------------------------------remove whites spaces from array or list-----------
'where textArray is the array varaible you want to removes spaces
textArray.Where(Function(x) Not String.IsNullOrWhiteSpace(x)).ToArray

'--------------------------------------------------------------------------------------

'---------------------------------------------------------------------------------------------------------
convert string to a date time

Convert.toDatetime(Datetime.parseexact( StringRequiredDateTimeStamp,in_ExternalDictionary("DataBaseDateTimeFormat").ToString,system.globalization.cultureinfo.invariantculture))

'---------------------------------------------------------------------------------------------
Remove element in an array by its name..using the string instaed of index

io_ArrayToUpdate.Where(Function(s) s <> in_FileToRemove).ToArray
'----------------------------------------------------------------------------
Get all available column index of a datatable 

(From DataColumnFound In  out_dtInputData.Columns.Cast(Of DataColumn)
Select DataColumnFound.Ordinal).ToList


'--------------------------------------------------------------------------
Invoke Code to merge two tables...merge parent into child table, copy and paste entire code in an invoke code

Try
Console.WriteLine("Running Invoke code to compile or merge datatables ")
If ParentTable IsNot Nothing AndAlso ParentTable.Rows.Count > 0
Console.WriteLine("parent table has "+ParentTable.Rows.Count.ToString+ " records ")
If ChildTable Is Nothing Then
	Console.WriteLine("data table to merge not  initialized...now initializing by cloning parent")
ChildTable = ParentTable.Clone
Console.WriteLine("child datatable cloned successfully")
Else
	Console.WriteLine("child data table  already initialized and has  "+ChildTable.Rows.Count.ToString+ " records ")
End If
Console.WriteLine("now merging parent data of "+ParentTable.Rows.Count.ToString+" into child datatable of "+ChildTable.Rows.Count.ToString+" records")
ChildTable.Merge(ParentTable, False, MissingSchemaAction.Ignore)
Console.WriteLine("merge successfully..Total number of records in child table is now  "+ChildTable.Rows.Count.ToString)
Console.WriteLine("Now clearing parent table ")
ParentTable.Clear()

Else
Console.WriteLine("No record in parent table ")
End If
Catch ex As Exception
	errorMessage = ex.Message
End Try


'------------------------------------------------------------------------

Trim spaces in data table
Removes spaces from the datable table dtRaw

'first initialize data table

TempData = dtRaw.Clone

'then do this in another assign

TempData = (From r In dtRaw.AsEnumerable
Select ia = r.ItemArray.toList
Select ic = ia.ConvertAll(Function (e) e.ToString.Trim).toArray()
Select TempData.Rows.Add(ic)).CopyToDataTable()





'-----Sum column
replacedebitTxn.AsEnumerable.Sum(Function(x) Convert.ToDouble(x("TXN_AMT").ToString.Trim) ).ToString

DT.AsEnumerable.Sum(Function(a)if(String.IsNullOrEmpty(a(“Column A”).ToString) or String.IsNullOrWhiteSpace(a(“Column A”).ToString),0,Convert.ToDouble(a(“Column A”).ToString)))
'---------------------------------------------------------------------------
'Filter table
Dim TempDt As System.Data.DataTable = S4spooldt.AsEnumerable().Where(Function(r) r(TradeIdColumnIndex).ToString.StartsWith(TradeID)).CopyToDataTable

'---------
Regex to remove special characters 
System.Text.RegularExpressions.Regex.Replace(variable, “[^a-z A-Z 0-9]”, “”)

'------------------get sum----------------------------
replacedebitTxn.AsEnumerable.Sum(Function(x) Convert.ToDouble(x("TXN_AMT").ToString.Trim) ).ToString

'-----skip data table and take a certain chunk---
in_DataBaseDt.AsEnumerable.Skip(RowCounter).Take(CheckDBChunkSize).CopyToDatatable()

'----reorder dictionary by value
columnOrderBook = columnOrderBook.OrderBy(Function (x) x.Value(1)).ToDictionary(Function (x) x.Key,Function (x) x.Value)

'---------------------------------
get index of all colums
Dim ListOfColumnIndexesFound As List(Of Int32) = (From DataColumnFound In  dt.Columns.Cast(Of DataColumn) Select DataColumnFound.Ordinal).ToList

'-----------------------------------------

-sums up amount column of like rows in a table as a new row in a new data table

dtResult = (From d In  dtData.AsEnumerable Group d By  k1=d(0).toString, k2=d(1).toString.Trim Into grp=Group Let s = grp.Sum(Function (x) CDbl(x(2).toString.Trim)) Let ra = New Object(){k1,k2,s,grp.First()(3)} Select dtResult.Rows.Add(ra)).CopyToDataTable


'------------

Remove empty spaces on table
dtCorrected = (From r In dt.AsEnumerable Select ia = r.ItemArray.ToList Select ic = ia.ConvertAll(Function (e) System.Text.RegularExpressions.Regex.Replace(e.ToString.Trim.Replace(" ",String.Empty), "[^a-z A-Z 0-9]", String.Empty)).toArray() Select dtCorrected.Rows.Add(ic)).CopyToDataTable()
		dt = dtCorrected


(New System.Net.NetworkCredential("",ExternalDictionary("UnifiedPaymentPassword").ToString)).SecurePassword

''convert column to string list

milliDT.AsEnumerable().[Select](Function(x) x(0).ToString()).Aggregate(Function(a, b) String.Concat(a, "','" & b))

 new System.Net.NetworkCredential(string.Empty, passwordSecure).Password
new System.Net.NetworkCredential(string.Empty, passwordSecure).Password

'-------------------------
hardcode a dictionary
new Dictionary(Of String, String)From {{“0”, “string”}, {“1”, “string2”}}


'-------------------------get duplicates from a table ---------------------------
If dt IsNot Nothing AndAlso dt.Rows.Count > 0
Try
Console.WriteLine("Extracting entries that match by narration")
Duplicate  = dt.AsEnumerable().
           GroupBy(Function (row) New With
           {
           Key .REF = CStr(row("BotUniqueID")),
           Key .ABS = Math.Abs(CDbl(row("LCY_AMOUNT")))
           }).
       Where (Function(Group) (Group.Count() > 1)).ToList.SelectMany(Function(m) m).CopyToDataTable()
   Catch ex As Exception
       errorMessage = ex.Message
   End Try
End If
'---------------------------------------------------- ---------------------------


Regex.Replace(STR, " {2,}", " ")
'-----------------get non duplicates-----------------------------------------------
If dt IsNot Nothing AndAlso dt.Rows.Count > 0
Try
Console.WriteLine("Extracting entries that do not match by narration")
NonDuplicates  = dt.AsEnumerable().
           GroupBy(Function (row) New With
           {
           Key .REF = CStr(row("BotUniqueID")),
           Key .ABS = Math.Abs(CDbl(row("LCY_AMOUNT")))
           }).
       Where (Function(Group) (Group.Count() = 1)).ToList.SelectMany(Function(m) m).CopyToDataTable()
   Catch ex As Exception
       errorMessage = ex.Message
   End Try
End If

'----removes alll spaces in a data table without looping -----

	Dim dtCorrected As DataTable = dt.Clone
	'removes all spaces inside of cell
	dtCorrected = (From r In dt.AsEnumerable Select ia = r.ItemArray.toList Select ic = ia.ConvertAll(Function (e) e.ToString.Trim.Replace(" ",String.Empty)).toArray() Select dtCorrected.Rows.Add(ic)).CopyToDataTable()
	dt.Clear()

'---------------------------------------------------------------------------
'-------Convert datarow to a dictionary------------
row.Table.Columns.Cast(Of DataColumn)().Zip(row.ItemArray, Function(c, v) New With {.ColumnName = c.ColumnName, .Value = v}).ToDictionary(Function(item) item.ColumnName, Function(item) item.Value)

Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"Automation","OhioMediaidPortalDataExtractionDispatcher","Temps")
																																															
