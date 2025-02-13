// Copyright (c) Drew Noakes and contributors. All Rights Reserved. Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace MetadataExtractor.Formats.Exif.Makernotes;

/// <summary>
/// The Olympus equipment makernote is used by many manufacturers (Epson, Konica, Minolta and Agfa...), and as such contains some tags
/// that appear specific to those manufacturers.
/// </summary>
/// <author>Kevin Mott https://github.com/kwhopper</author>
/// <author>Drew Noakes https://drewnoakes.com</author>
public sealed class OlympusEquipmentMakernoteDirectory : Directory
{
    public const int TagEquipmentVersion = 0x0000;
    public const int TagCameraType2 = 0x0100;
    public const int TagSerialNumber = 0x0101;

    public const int TagInternalSerialNumber = 0x0102;
    public const int TagFocalPlaneDiagonal = 0x0103;
    public const int TagBodyFirmwareVersion = 0x0104;

    public const int TagLensType = 0x0201;
    public const int TagLensSerialNumber = 0x0202;
    public const int TagLensModel = 0x0203;
    public const int TagLensFirmwareVersion = 0x0204;
    public const int TagMaxApertureAtMinFocal = 0x0205;
    public const int TagMaxApertureAtMaxFocal = 0x0206;
    public const int TagMinFocalLength = 0x0207;
    public const int TagMaxFocalLength = 0x0208;
    public const int TagMaxAperture = 0x020A;
    public const int TagLensProperties = 0x020B;

    public const int TagExtender = 0x0301;
    public const int TagExtenderSerialNumber = 0x0302;
    public const int TagExtenderModel = 0x0303;
    public const int TagExtenderFirmwareVersion = 0x0304;

    public const int TagConversionLens = 0x0403;

    public const int TagFlashType = 0x1000;
    public const int TagFlashModel = 0x1001;
    public const int TagFlashFirmwareVersion = 0x1002;
    public const int TagFlashSerialNumber = 0x1003;

    private static readonly Dictionary<int, string> _tagNameMap = new()
    {
        { TagEquipmentVersion, "Equipment Version" },
        { TagCameraType2, "Camera Type 2" },
        { TagSerialNumber, "Serial Number" },

        { TagInternalSerialNumber, "Internal Serial Number" },
        { TagFocalPlaneDiagonal, "Focal Plane Diagonal" },
        { TagBodyFirmwareVersion, "Body Firmware Version" },

        { TagLensType, "Lens Type" },
        { TagLensSerialNumber, "Lens Serial Number" },
        { TagLensModel, "Lens Model" },
        { TagLensFirmwareVersion, "Lens Firmware Version" },
        { TagMaxApertureAtMinFocal, "Max Aperture At Min Focal" },
        { TagMaxApertureAtMaxFocal, "Max Aperture At Max Focal" },
        { TagMinFocalLength, "Min Focal Length" },
        { TagMaxFocalLength, "Max Focal Length" },
        { TagMaxAperture, "Max Aperture" },
        { TagLensProperties, "Lens Properties" },

        { TagExtender, "Extender" },
        { TagExtenderSerialNumber, "Extender Serial Number" },
        { TagExtenderModel, "Extender Model" },
        { TagExtenderFirmwareVersion, "Extender Firmware Version" },

        { TagConversionLens, "Conversion Lens" },

        { TagFlashType, "Flash Type" },
        { TagFlashModel, "Flash Model" },
        { TagFlashFirmwareVersion, "Flash Firmware Version" },
        { TagFlashSerialNumber, "Flash Serial Number" }
    };

    public OlympusEquipmentMakernoteDirectory() : base(_tagNameMap)
    {
        SetDescriptor(new OlympusEquipmentMakernoteDescriptor(this));
    }

    public override string Name => "Olympus Equipment";
}
