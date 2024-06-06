Imports Nevron.Nov
Imports Nevron.Nov.Examples
Imports Nevron.Nov.Graphics
Imports Nevron.Nov.Layout
Imports Nevron.Nov.UI
Imports Nevron.Nov.Windows.Forms

Public Class Form1

#Region "Constructors"

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        ' Set form title
        Text = "NOV WinForms - {Title}"

        ' Create the NOV example and its controls
        Dim exampleContent As NWidget = CreateExampleContent()
        Dim exampleControls As NWidget = CreateExampleControls()
        If Not exampleControls Is Nothing Then
            exampleControls.PreferredWidth = 300
        End If

        ' Place the example and its controls in a pair box
        Dim pairBox As NPairBox = New NPairBox(exampleContent, exampleControls)
        pairBox.FillMode = ENStackFillMode.First
        pairBox.FitMode = ENStackFitMode.First
        pairBox.Margins = New NMargins(NDesign.HorizontalSpacing, NDesign.VerticalSpacing)

        ' Host the example in the form
        Dim host As NNovWidgetHost(Of NWidget) = New NNovWidgetHost(Of NWidget)(pairBox)
        host.Dock = DockStyle.Fill
        Controls.Add(host)
    End Sub


#End Region

#Region "Example"

    Private Function CreateExampleControls() As NWidget
        Return Nothing
    End Function

    Private Function CreateExampleContent() As NWidget
        Return Nothing
    End Function

#End Region

#Region "Implementation"
#End Region

#Region "Event Handlers"
#End Region

#Region "Fields"
#End Region

#Region "Static Methods"
#End Region

#Region "Constants"
#End Region

#Region "Nested Types"
#End Region

End Class