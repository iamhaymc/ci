using System.IO;
using System.Text;

namespace Trane.Submittals.Pipeline
{
  public interface IMediaConverter
  {
    bool ConvertEmfToSvg(string emfPath, string svgPath);
    bool ConvertEmfToSvg(Stream emfStream, Stream svgStream);
    string ConvertEmfToSvg(Stream emfStream);
    bool ConvertDxfToSvg(string dxfPath, string svgPath);
    bool ConvertDxfToSvg(Stream dxfStream, Stream svgStream);
    string ConvertDxfToSvg(Stream dxfStream);
  }

  public class MediaConverter : IMediaConverter
  {
    #region [License]

    private static bool _isLicensed = false;
    private static object _licenseLock = new object();
    private static void ApplyLicense()
    {
      lock (_licenseLock)
      {
        if (_isLicensed) return;
        _isLicensed = true;

        string license = @"
<?xml version=""1.0""?>
<License>
  <Data>
    <LicensedTo>Ingersoll Rand Co Trane</LicensedTo>
    <EmailTo>purchasing@irco.com</EmailTo>
    <LicenseType>Site OEM</LicenseType>
    <LicenseNote>Up To 10 Developers And Unlimited Deployment Locations</LicenseNote>
    <OrderID>210610235855</OrderID>
    <UserID>418764</UserID>
    <OEM>This is a redistributable license</OEM>
    <Products>
      <Product>Aspose.Total for .NET</Product>
    </Products>
    <EditionType>Enterprise</EditionType>
    <SerialNumber>22c3d588-f787-49e9-93e3-95215916cc04</SerialNumber>
    <SubscriptionExpiry>20220610</SubscriptionExpiry>
    <LicenseVersion>3.0</LicenseVersion>
    <LicenseInstructions>https://purchase.aspose.com/policies/use-license</LicenseInstructions>
  </Data>
  <Signature>gTYZmAgMkvT+RQmg6kmndhBZsnH2YJrymwiyJ2jwnzZ3WX5gfT7V1J4lb4/6DegBQ+XhLYvxzRzfeX/z2agT2syAMWqECMT4AiICx2I8gbk/JDvt7BziHRWkplCZLTbD3SrCW3D38Ya11GBK5n8bjoTvtqehvuitG/Kq0g+cfAk=</Signature>
</License>
".Trim();
        using (MemoryStream licenseStream = new MemoryStream(Encoding.UTF8.GetBytes(license)))
        {
          // licenseStream.Position = 0;
          // new Aspose.ThreeD.License().SetLicense(licenseStream);
          // licenseStream.Position = 0;
          // new Aspose.BarCode.License().SetLicense(licenseStream);
          licenseStream.Position = 0;
          new Aspose.CAD.License().SetLicense(licenseStream);
          licenseStream.Position = 0;
          // new Aspose.Diagram.License().SetLicense(licenseStream);
          // licenseStream.Position = 0;
          // new Aspose.Drawing.License().SetLicense(licenseStream);
          // licenseStream.Position = 0;
          // new Aspose.Gis.License().SetLicense(licenseStream);
          licenseStream.Position = 0;
          new Aspose.Imaging.License().SetLicense(licenseStream);
          // licenseStream.Position = 0;
          // new Aspose.Pdf.License().SetLicense(licenseStream);
          // licenseStream.Position = 0;
          // new Aspose.Svg.License().SetLicense(licenseStream);
          // licenseStream.Position = 0;
          // new Aspose.TeX.License().SetLicense(licenseStream);
          // licenseStream.Position = 0;
          // new Aspose.Words.License().SetLicense(licenseStream);
        };
      }
    }

    #endregion

    public MediaConverter()
    {
      if (!_isLicensed) ApplyLicense();
    }

    public bool ConvertEmfToSvg(string emfPath, string svgPath)
    {
      using (Aspose.Imaging.Image emfImage = Aspose.Imaging.Image.Load(emfPath))
      {
        emfImage.Save(svgPath, ConfigureEmfToSvg(emfImage));
      }
      return true;
    }

    public bool ConvertEmfToSvg(Stream emfStream, Stream svgStream)
    {
      using (Aspose.Imaging.Image emfImage = Aspose.Imaging.Image.Load(emfStream))
      {
        emfImage.Save(svgStream, ConfigureEmfToSvg(emfImage));
      }
      return true;
    }

    public string ConvertEmfToSvg(Stream emfStream)
    {
      string svgText = null;
      using (MemoryStream svgStream = new MemoryStream())
      {
        using (Aspose.Imaging.Image emfImage = Aspose.Imaging.Image.Load(emfStream))
        {
          emfImage.Save(svgStream, ConfigureEmfToSvg(emfImage));
        }
        svgStream.Position = 0;
        svgText = Encoding.UTF8.GetString(svgStream.ToArray());
      }
      return svgText;
    }

    private Aspose.Imaging.ImageOptions.SvgOptions ConfigureEmfToSvg(Aspose.Imaging.Image emfImage)
    {
      return new Aspose.Imaging.ImageOptions.SvgOptions
      {
        VectorRasterizationOptions = new Aspose.Imaging.ImageOptions.EmfRasterizationOptions
        {
          PageSize = new Aspose.Imaging.SizeF(emfImage.Width, emfImage.Height),
          SmoothingMode = Aspose.Imaging.SmoothingMode.HighQuality,
          TextRenderingHint = Aspose.Imaging.TextRenderingHint.ClearTypeGridFit,
          RenderMode = Aspose.Imaging.FileFormats.Emf.EmfRenderMode.Dual,
          ResolutionSettings = new Aspose.Imaging.ResolutionSetting { HorizontalResolution = 100, VerticalResolution = 100 },
          BackgroundColor = Aspose.Imaging.Color.Transparent,
        },
        ColorType = Aspose.Imaging.FileFormats.Svg.SvgColorMode.Rgb,
        ResolutionSettings = new Aspose.Imaging.ResolutionSetting { HorizontalResolution = 100, VerticalResolution = 100 },
        TextAsShapes = false,
      };
    }

    public bool ConvertDxfToSvg(string dxfPath, string svgPath)
    {
      using (Aspose.CAD.Image cadImage = Aspose.CAD.Image.Load(dxfPath))
      {
        cadImage.Save(svgPath, ConfigureDxfToSvg(cadImage));
      }
      return true;
    }

    public bool ConvertDxfToSvg(Stream dxfStream, Stream svgStream)
    {
      using (Aspose.CAD.Image cadImage = Aspose.CAD.Image.Load(dxfStream))
      {
        cadImage.Save(svgStream, ConfigureDxfToSvg(cadImage));
      }
      return true;
    }

    public string ConvertDxfToSvg(Stream dxfStream)
    {
      string svgText = null;
      using (MemoryStream svgStream = new MemoryStream())
      {
        using (Aspose.CAD.Image cadImage = Aspose.CAD.Image.Load(dxfStream))
        {
          cadImage.Save(svgStream, ConfigureDxfToSvg(cadImage));
        }
        svgStream.Position = 0;
        svgText = Encoding.UTF8.GetString(svgStream.ToArray());
      }
      return svgText;
    }

    private Aspose.CAD.ImageOptions.SvgOptions ConfigureDxfToSvg(Aspose.CAD.Image cadImage)
    {
      return new Aspose.CAD.ImageOptions.SvgOptions
      {
        VectorRasterizationOptions = new Aspose.CAD.ImageOptions.CadRasterizationOptions
        {
          AutomaticLayoutsScaling = true,
          ScaleMethod = Aspose.CAD.FileFormats.Cad.ScaleType.ShrinkToFit,
          Quality = new Aspose.CAD.ImageOptions.RasterizationQuality
          {
            ObjectsPrecision = Aspose.CAD.ImageOptions.RasterizationQualityValue.High,
            Arc = Aspose.CAD.ImageOptions.RasterizationQualityValue.High,
            Hatch = Aspose.CAD.ImageOptions.RasterizationQualityValue.High,
            Text = Aspose.CAD.ImageOptions.RasterizationQualityValue.High,
            TextThicknessNormalization = true
          },
          GraphicsOptions = new Aspose.CAD.ImageOptions.GraphicsOptions
          {
            SmoothingMode = Aspose.CAD.SmoothingMode.HighQuality,
            InterpolationMode = Aspose.CAD.InterpolationMode.HighQualityBicubic,
            TextRenderingHint = Aspose.CAD.TextRenderingHint.ClearTypeGridFit
          },
          DrawColor = Aspose.CAD.Color.Black,
          BackgroundColor = Aspose.CAD.Color.Transparent,
        },
        ColorType = Aspose.CAD.ImageOptions.SvgOptionsParameters.SvgColorMode.Rgb,
        ResolutionSettings = new Aspose.CAD.ResolutionSetting { HorizontalResolution = 100, VerticalResolution = 100 },
        TextAsShapes = false,
      };
    }
  }
}
