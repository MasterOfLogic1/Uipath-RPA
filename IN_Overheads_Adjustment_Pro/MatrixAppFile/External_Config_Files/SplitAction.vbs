'
' This program splits an 835 by NPI
'
' e.g., cscript 835Split.vbs file
'
' or drag and drop a file onto this script
'
' Originally written by Forefront Technologies.
' http://forefronttechnologiesonline.com
'
' You can use this program for your own use as long
' as you keep the comment lines above, intact.
'

    OPTION EXPLICIT

    '
    ' Define variables and constants
    '

    Const ForReading = 1, ForWriting = 2, ForAppending = 8
    Const TristateFalse = 0

    dim myApp
    myApp = mid(wscript.scriptname,1,len(wscript.scriptname)-len(".vbs"))

    dim delimEOL
    dim delimSubFld
    dim delimFld

    dim ISAptr
    dim IEAptr

    dim STptr
    dim SEptr

    dim lastISAptr
    dim lastSTptr
    
    dim ISAWritten

    dim filePath
    dim fileName

    dim fso
    dim ifo
    dim its
    dim ots

    dim inpFile
    dim inpLine

    dim ii

    dim tmpStr
    dim tmpArray

    dim npiList
    dim npi
    dim npiCtr

    '
    ' Start of processing
    '

    on error goto 0
	inpFile = WScript.Arguments.Item(0)
	'inpFile = WScript.Arguments(0)
	'WScript.Echo inpFile
	'inpFile = "C:\Users\oajisafe\OneDrive - Help at Home\Documents\UiPath\GainwellFileUpload\R824.835.X.084340.112.dat"
    ii = instrrev(inpFile,"\")
    filePath = mid(inpFile,1,ii)
    fileName = mid(inpFile,ii+1)
    '
    ' Open the input file and read it in.
    '    
    Set fso = CreateObject("Scripting.FileSystemObject")
    set ifo = fso.GetFile(inpFile)
    set its = ifo.OpenAsTextStream(ForReading, TristateFalse)
    inpLine = its.Read(ifo.Size)
    '
    ' Pull out the delimitors from their fixed position in the first line.
    '
    delimEOL = mid(inpLine,106,1)
    delimSubFld = mid(inpLine,105,1)
    delimFld = mid(inpLine,104,1)
    '
    ' Drop all the line feeds and carrige returns (if there are any)
    ' then split the lines into an array of lines.
    '
    inpLine = Replace(inpLine,vbLf,"")
    inpLine = Replace(inpLine,vbCr,"")
    inpLine = split(inpLine,delimEOL)
    '
    ' Get the list of NPIs from the file.
    '
    npiList = ""
    for ii = 0 to ubound(inpLine)
        if ucase(mid(inpLine(ii),1,6)) = "TRN" & delimFld & "1" & delimFld then
            tmpArray = split(inpLine(ii),delimFld)
            tmpStr = tmpArray(2)
            if instr(npiList,tmpStr) = 0 then
                npiList = npiList & delimFld & tmpStr
            end if
        end if
    next
    '
    ' Split the list of NPIs into an array of NPIs.
    '
    logit npiList
    npi = split(mid(npiList,2),delimFld)
    npiCtr = split(mid(npiList,2),delimFld)
    for ii = 0 to ubound(npiCtr)
        npiCtr(ii) = 0
    next
    '
    ' Create a file for each NPI and split its data out into its own file.
    '
    for ii = 0 to ubound(npi)
        Set ots = fso.CreateTextFile(filePath & npi(ii) & ".tmp",true,false)
        splitOut npi(ii), npiCtr(ii)
        ots.close
    next
    '
    ' Rename all the files to their final names.
    '
	Dim fileList
	'Set fileList = CreateObject("System.Collections.ArrayList")
    for ii = 0 to ubound(npi)
        fso.movefile filePath & npi(ii) & ".tmp", filePath & npi(ii) & "_" & fileName & ".txt"
		fileList = fileList+"|"+(filePath & npi(ii) & "_" & fileName & ".txt")
    next
    its.close
    fso.movefile inpFile, inpFile & ".split"
	WScript.Echo fileList
    '
    ' All done.
    '
    set its = Nothing
    set ots = Nothing
    Set fso = Nothing
    logit myApp & " end"


function splitOut(npi,npiCtr)
    '
    ' This function splits out the data for a specific NPI and write it to the output file.
    '
    dim ii
    dim tmpArray

    lastISAptr = -1
    '
    ' Do this for each ISA/IEA pair in the file.
    '
    do while getISA(inpLine,ISAptr,IEAptr,lastISAptr)
        ISAWritten = false
        lastSTptr = ISAptr
        '
        ' Do this for each ST/SE pair in this ISA/IEA pair.
        '
        do while getST(inpLine,IEAptr,STptr,SEptr,lastSTptr)
            '
            ' If this ST/SE pair is for the NPI we're working on
            ' then write it to the output file.
            '
            if forNPI(npi,inpLine,STptr,SEptr) then
                npiCtr = npiCtr + 1
                if not ISAWritten then
                    for ii = ISAptr to ISAptr+1
                        writeLine npiCtr, inpLine(ii)
                    next
                    ISAWritten = true
                end if
                for ii = STptr to SEptr
                     writeLine npiCtr, inpLine(ii)
                next
            end if
        loop
        if ISAWritten then
            for ii = IEAptr-1 to IEAptr
                writeLine npiCtr, inpLine(ii)
            next
        end if
    loop
    if ISAWritten then
        for ii = SEptr+1 to IEAptr
            writeLine npiCtr, inpLine(ii)
        next
    end if
end function


function writeLine(npiCtr, outLine)
    '
    ' This function writes a line to the output file
    ' with the end of line terminator on the end.
    '
    ' If the line we're writing is the GE line,
    ' update the number of code sets before writing it
    '
    dim tmpArray
    dim tmpStr

    if mid(ucase(outLine),1,3) = "GE" & delimFld then
        tmpArray = split(outLine,delimFld)
        tmpArray(1) = npiCtr
        tmpStr = join(tmpArray,delimFld)
        ots.WriteLine tmpStr & delimEOL
    else
        ots.WriteLine trim(outLine) & delimEOL
    end if
end function
   

function getISA(inpLine,ISAptr,IEAptr,lastISAptr)
    '
    ' This function looks through the input lines starting from where we last left off
    ' looking for the next ISA/IEA pair and returns pointers to them.
    '
    dim ii

    ISAptr = -1
    IEAptr = -1
    getISA = false
    do while lastISAptr < ubound(inpLine)
        lastISAptr = lastISAptr + 1
        if ucase(mid(trim(inpLine(lastISAptr)),1,4)) = "ISA" & delimFld then
            ISAptr = lastISAptr
        elseif ucase(mid(trim(inpLine(lastISAptr)),1,4)) = "IEA" & delimFld then
            IEAptr = lastISAptr
            exit do
        end if
    loop
    if ISAptr > -1 and IEAptr > -1 then
        getISA = true
    end if
end function

function getST(inpLine,IEAptr,STptr,SEptr,lastSTptr)
    '
    ' This function looks through the input lines starting from where we last left off
    ' looking for the next ST/SE pair and returns pointers to them.
    '
    dim ii

    STptr = -1
    SEptr = -1
    getST = false
    do while lastSTptr <= IEAptr
        lastSTptr = lastSTptr + 1
        if ucase(mid(trim(inpLine(lastSTptr)),1,3)) = "ST" & delimFld then
            STptr = lastSTptr
        elseif ucase(mid(trim(inpLine(lastSTptr)),1,3)) = "SE" & delimFld then
            SEptr = lastSTptr
            exit do
        end if
    loop
    if STptr > -1 and SEptr > -1 then
        getST = true
    end if
end function

function forNPI(npi,inpLine,ISAptr,IEAptr)
    '
    ' This function looks through the lines of the current ST/SE pair
    ' and checks if it is for the current NPI we're working on.
    '
    dim ii
    dim tmpArray

    for ii = ISAptr to IEAptr
        if ucase(mid(inpLine(ii),1,6)) = "TRN" & delimFld & "1" & delimFld then
            tmpArray = split(trim(inpLine(ii)),delimFld)
            if tmpArray(2) = npi then
                forNPI = true
            else
                forNPI = false
            end if
            exit function
        end if
    next
    forNPI = false
end function


function logit(Msg)
   '' wscript.echo mySQLDate(Date) & " " & mySQLTime(Time) & ": " & Msg
end function

function mySQLDate(iDate)
    if iDate > "" then
        mySQLDate = year(iDate) & "-" & right("0" & month(iDate),2) & "-" & right("0" & day(iDate),2)
    end if
end function

function mySQLTime(iTime)
    on error resume next
    mySQLTime = ""
    if ucase(trim(iTime)) = "AM" or ucase(trim(iTime)) = "PM" then
        mySQLTime = ""
    elseif trim(iTime) > "" then
        if instr(ucase(iTime),"AM") then
            if hour(iTime) < 13 then
                mySQLTime = right("0" & hour(iTime),2) & ":" & right("0" & minute(iTime),2) & ":" & right("0" & second(iTime),2)
            else
                mySQLTime = right("0" & (hour(iTime) mod 12),2) & ":" & right("0" & minute(iTime),2) & ":" & right("0" & second(iTime),2)
            end if
        elseif instr(ucase(iTime),"PM") then
            mySQLTime = right("0" & (12+(hour(iTime) mod 12)),2) & ":" & right("0" & minute(iTime),2) & ":" & right("0" & second(iTime),2)
        else
            mySQLTime = right("0" & hour(iTime),2) & ":" & right("0" & minute(iTime),2) & ":" & right("0" & second(iTime),2)
        end if
    end if
    on error goto 0
end function
