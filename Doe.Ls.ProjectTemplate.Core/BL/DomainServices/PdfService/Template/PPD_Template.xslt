<xsl:stylesheet version="1.0" xmlns:xsl=
  "http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml"
    doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-
transitional.dtd"
    doctype-public="-//W3C//DTD XHTML 1.0 Transitional//
EN" indent="yes"/>



  <xsl:template match="/">
    <html>
      <head>
        <link href="PDFGenerator.css" rel="stylesheet" />
      </head>
      <body>
        <table class="PDTableHeader">
          <tr>
            <td>
              <h1>Position Description</h1>
            </td>
            <td align="right">

              <xsl:element name="img">
                <xsl:attribute name="src">
                  <xsl:value-of select="Position/Admin/Logo"/>
                </xsl:attribute>
                <xsl:attribute name="width">170</xsl:attribute>
                <xsl:attribute name="height">54</xsl:attribute>

              </xsl:element>

            </td>
          </tr>
        </table>

        <table class="PDTable">
          <tr>
            <th>Division</th>
            <td>
              <xsl:value-of select="Position/@Division" />
            </td>
          </tr>
          <tr>
            <th>Directorate</th>
            <td>
              <xsl:value-of select="Position/@Directorate" />
            </td>
          </tr>
          <tr>
            <th>Business Unit</th>
            <td>
              <xsl:value-of select="Position/@BusinessUnit" />
            </td>
          </tr>
          <tr>
            <th>Position Number</th>
            <td>
              <xsl:value-of select="Position/@PositionNumber" />
            </td>
          </tr>
          <tr>
            <th>Title</th>
            <td>
              <xsl:value-of select="Position/@Title" />
            </td>
          </tr>
          <tr>
            <th>Classification</th>
            <td>
              <xsl:value-of select="Position/@GradeCode" />
            </td>
          </tr>
          <tr>
            <th>Brief Role Statement</th>
            <td>
              <xsl:value-of select="Position/@BriefRoleStatement" />
            </td>
          </tr>
          <tr>
            <th>Statement of Duties</th>
            <td>
              <xsl:value-of select="Position/StatementOfDuties"  disable-output-escaping="yes"/>
            </td>
          </tr>
          <tr>
            <th>Selection Criteria</th>
            <td>
              <ul>
                <xsl:for-each select="Position/SelectionCriteria/Criteria">
                  <li>
                    <xsl:value-of select="."/>
                  </li>
                </xsl:for-each>
              </ul>
            </td>
          </tr>
        </table>
        <table class="PDTableFooter" border="0">
          <tr>
            <td align="left">
              <xsl:value-of select="Position/Admin/DocNumber"/>
            </td>
            <td align="right">
              Updated:   <xsl:value-of select="Position/Admin/ApprovedDate"/>
            </td>

          </tr>
        </table>
      </body>
    </html>
  </xsl:template>

</xsl:stylesheet>