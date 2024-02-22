Imports System
Imports System.IO
Imports System.Net
Imports System.Security
Imports System.Text
Imports Newtonsoft.Json.Linq

Module Program
    Sub Main(args As String())
        Dim pw As String
        Dim shaPW As String = ""
        Dim userWantsToExit As Boolean = True

        While (userWantsToExit)
            Console.Write("capturePW: ")
            pw = Console.ReadLine()

            If pw = "exit" Then
                userWantsToExit = False
            End If

            Console.WriteLine(sha1PW(pw))

            Console.WriteLine("pwned: " & pwnedPW(sha1PW(pw)))
        End While
    End Sub

    Function sha1PW(ByVal pw As String) As String
        Dim sha1Obj As New Cryptography.SHA1CryptoServiceProvider
        Dim bytesToHash() As Byte = Encoding.ASCII.GetBytes(pw)

        bytesToHash = sha1Obj.ComputeHash(bytesToHash)

        For Each b As Byte In bytesToHash
            sha1PW += b.ToString("x2")
        Next
        Return sha1PW
    End Function

    Function pwnedPW(ByVal sha1 As String) As Boolean
        Dim request As WebRequest = WebRequest.Create("https://pwned.gobdigital.com/ispwned/" + sha1)
        ' Get the response.
        Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
        ' Get the stream containing content returned by the server.
        Dim dataStream As Stream = response.GetResponseStream()
        ' Open the stream using a StreamReader for easy access.
        Dim reader As New StreamReader(dataStream)
        ' Read the content.
        Dim responseFromServer As String = reader.ReadToEnd()

        pwnedPW = JObject.parse(responseFromServer)("pwned")
    End Function
End Module
