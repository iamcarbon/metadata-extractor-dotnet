// Copyright (c) Drew Noakes and contributors. All Rights Reserved. Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace MetadataExtractor.Formats.Exif.Makernotes;

/// <summary>Describes tags specific to Sigma / Foveon cameras.</summary>
/// <author>Drew Noakes https://drewnoakes.com</author>
public class SigmaMakernoteDirectory : Directory
{
    public const int TagSerialNumber = 0x2;
    public const int TagDriveMode = 0x3;
    public const int TagResolutionMode = 0x4;
    public const int TagAutoFocusMode = 0x5;
    public const int TagFocusSetting = 0x6;
    public const int TagWhiteBalance = 0x7;
    public const int TagExposureMode = 0x8;
    public const int TagMeteringMode = 0x9;
    public const int TagLensRange = 0xa;
    public const int TagColorSpace = 0xb;
    public const int TagExposure = 0xc;
    public const int TagContrast = 0xd;
    public const int TagShadow = 0xe;
    public const int TagHighlight = 0xf;
    public const int TagSaturation = 0x10;
    public const int TagSharpness = 0x11;
    public const int TagFillLight = 0x12;
    public const int TagColorAdjustment = 0x14;
    public const int TagAdjustmentMode = 0x15;
    public const int TagQuality = 0x16;
    public const int TagFirmware = 0x17;
    public const int TagSoftware = 0x18;
    public const int TagAutoBracket = 0x19;

    private static readonly Dictionary<int, string> _tagNameMap = new()
    {
        { TagSerialNumber, "Serial Number" },
        { TagDriveMode, "Drive Mode" },
        { TagResolutionMode, "Resolution Mode" },
        { TagAutoFocusMode, "Auto Focus Mode" },
        { TagFocusSetting, "Focus Setting" },
        { TagWhiteBalance, "White Balance" },
        { TagExposureMode, "Exposure Mode" },
        { TagMeteringMode, "Metering Mode" },
        { TagLensRange, "Lens Range" },
        { TagColorSpace, "Color Space" },
        { TagExposure, "Exposure" },
        { TagContrast, "Contrast" },
        { TagShadow, "Shadow" },
        { TagHighlight, "Highlight" },
        { TagSaturation, "Saturation" },
        { TagSharpness, "Sharpness" },
        { TagFillLight, "Fill Light" },
        { TagColorAdjustment, "Color Adjustment" },
        { TagAdjustmentMode, "Adjustment Mode" },
        { TagQuality, "Quality" },
        { TagFirmware, "Firmware" },
        { TagSoftware, "Software" },
        { TagAutoBracket, "Auto Bracket" }
    };

    public SigmaMakernoteDirectory() : base(_tagNameMap)
    {
        SetDescriptor(new SigmaMakernoteDescriptor(this));
    }

    public override string Name => "Sigma Makernote";
}
