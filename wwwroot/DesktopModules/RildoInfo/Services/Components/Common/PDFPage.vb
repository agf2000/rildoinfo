Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Web.Hosting

Public Class pdfPage
    Inherits PdfPageEventHelper

    'I create a font object to use within my footer
    Protected Shared ReadOnly Property footer() As Font
        Get
            ' create a basecolor to use for the footer font, if needed.
            Dim grey As New BaseColor(128, 128, 128)
            Dim font As Font = FontFactory.GetFont("Arial", 8, font.NORMAL, grey)
            Return font
        End Get
    End Property

    'override the OnStartPage event handler to add our header
    Public Overloads Overrides Sub OnStartPage(writer As PdfWriter, doc As Document)

        If HttpContext.Current.Request.Params("customerLogo") IsNot Nothing Then

            Dim heading12 As Font = FontFactory.GetFont("ARIAL", 12)
            Dim heading10 As Font = FontFactory.GetFont("ARIAL", 10)
            Dim heading9 As Font = FontFactory.GetFont("VERDANA", 9)
            Dim heading8 As Font = FontFactory.GetFont("VERDANA", 8)
            Dim heading8b As Font = FontFactory.GetFont("VERDANA", 8, Font.BOLD)
            Dim heading7 As Font = FontFactory.GetFont("VERDANA", 7)

            Dim logoImage As String = HostingEnvironment.MapPath(CStr(HttpContext.Current.Request.Params("customerLogo")))
            Dim jpgLogo As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(logoImage)
            jpgLogo.ScaleToFit(180.0F, 68.0F)

            Dim headerTitle As Paragraph = New Paragraph(HttpContext.Current.Request.Params("PageHeader"), heading12) With {.Alignment = Element.ALIGN_RIGHT}
            Dim headerText As New Paragraph(HttpContext.Current.Request.Params("PageSubHeader").Replace("[NEWLINE]", Environment.NewLine()), footer) With {.Alignment = Element.ALIGN_RIGHT}

            Dim tableHeader = New PdfPTable(2) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .TotalWidth = doc.PageSize.Width}

            Dim cellHeaderLogo = New PdfPCell() With {.PaddingLeft = 20.0F, .Border = 0}
            cellHeaderLogo.AddElement(jpgLogo)

            Dim cellHeaderText = New PdfPCell() With {.PaddingRight = 20.0F, .Border = 0}
            cellHeaderText.AddElement(headerTitle)
            cellHeaderText.AddElement(headerText)

            tableHeader.AddCell(cellHeaderLogo)
            tableHeader.AddCell(cellHeaderText)

            tableHeader.WriteSelectedRows(0, -1, 0, (doc.PageSize.Height - 20), writer.DirectContent)

        End If

    End Sub

    'override the OnPageEnd event handler to add our footer
    Public Overloads Overrides Sub OnEndPage(writer As PdfWriter, doc As Document)
        'I use a PdfPtable with 2 columns to position my footer where I want it
        'set the width of the table to be the same as the document
        'Center the table on the page

        If HttpContext.Current.Request.Params("footerText") IsNot Nothing Then

            Dim footerTbl As New PdfPTable(3) With {.TotalWidth = doc.PageSize.Width, .HorizontalAlignment = Element.ALIGN_CENTER, .LockedWidth = True}
            Dim widthstable As Single() = New Single() {1.3F, 1.8F, 1.0F}
            footerTbl.SetWidths(widthstable)

            'Create a paragraph that contains the footer text
            Dim para As New Paragraph(CStr(HttpContext.Current.Request.Params("footerText")), footer)

            'add a carriage return
            para.Add(Environment.NewLine)
            para.Add(DateTime.Now.ToString("dd-MMM-yyyy"))

            'create a cell instance to hold the text
            'set cell border to 0
            'add some padding to bring away from the edge
            Dim cell As New PdfPCell(para) With {.Border = 0, .PaddingLeft = 10}

            'add cell to table
            footerTbl.AddCell(cell)

            para = New Paragraph(CStr(HttpContext.Current.Request.Params("customerFooter")).Replace("[NEWLINE]", Environment.NewLine()), footer)

            cell = New PdfPCell(para) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Border = 0}
            footerTbl.AddCell(cell)

            'create new instance of Paragraph for 2nd cell text
            para = New Paragraph(String.Format("Página {0}", doc.PageNumber), footer)

            'create new instance of cell to hold the text
            'align the text to the right of the cell
            'set border to 0
            ' add some padding to take away from the edge of the page
            cell = New PdfPCell(para) With {.HorizontalAlignment = Element.ALIGN_RIGHT, .Border = 0, .PaddingRight = 10}

            'add the cell to the table
            footerTbl.AddCell(cell)

            'write the rows out to the PDF output stream.
            footerTbl.WriteSelectedRows(0, -1, 0, (doc.BottomMargin + 20), writer.DirectContent)

        End If
    End Sub

End Class
