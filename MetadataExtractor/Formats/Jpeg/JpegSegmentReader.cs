﻿// Copyright (c) Drew Noakes and contributors. All Rights Reserved. Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#if NET35
using JpegSegmentList = System.Collections.Generic.IList<MetadataExtractor.Formats.Jpeg.JpegSegment>;
#else
using JpegSegmentList = System.Collections.Generic.IReadOnlyList<MetadataExtractor.Formats.Jpeg.JpegSegment>;
#endif

namespace MetadataExtractor.Formats.Jpeg;

/// <summary>Parses the structure of JPEG data, returning contained segments.</summary>
/// <remarks>
/// JPEG files are composed of a sequence of consecutive JPEG segments. Each segment has a type <see cref="JpegSegmentType"/>.
/// A JPEG file can contain multiple segments having the same type.
/// <para />
/// Segments are returned in the order they appear in the file, however that order may vary from file to file.
/// <para />
/// Use <see cref="ReadSegments(SequentialReader,ICollection{JpegSegmentType})"/> to specific segment types,
/// or pass <see langword="null" /> to read all segments.
/// <para />
/// Note that SOS (start of scan) or EOI (end of image) segments are not returned by this class's methods.
/// </remarks>
/// <seealso cref="JpegSegment"/>
/// <author>Drew Noakes https://drewnoakes.com</author>
public static class JpegSegmentReader
{
    /// <summary>
    /// Walks the provided JPEG data, returning <see cref="JpegSegment"/> objects.
    /// </summary>
    /// <remarks>
    /// Will not return SOS (start of scan) or EOI (end of image) segments.
    /// </remarks>
    /// <param name="filePath">a file from which the JPEG data will be read.</param>
    /// <param name="segmentTypes">the set of JPEG segments types that are to be returned. If this argument is <see langword="null" /> then all found segment types are returned.</param>
    /// <exception cref="JpegProcessingException"/>
    /// <exception cref="IOException"/>
    public static JpegSegmentList ReadSegments(string filePath, ICollection<JpegSegmentType>? segmentTypes = null)
    {
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return ReadSegments(new SequentialStreamReader(stream), segmentTypes).ToList();
    }

    /// <summary>
    /// Processes the provided JPEG data, and extracts the specified JPEG segments into a <see cref="JpegSegment"/> object.
    /// </summary>
    /// <remarks>
    /// Will not return SOS (start of scan) or EOI (end of image) segments.
    /// </remarks>
    /// <param name="reader">a <see cref="SequentialReader"/> from which the JPEG data will be read. It must be positioned at the beginning of the JPEG data stream.</param>
    /// <param name="segmentTypes">the set of JPEG segments types that are to be returned. If this argument is <see langword="null" /> then all found segment types are returned.</param>
    /// <exception cref="JpegProcessingException"/>
    /// <exception cref="IOException"/>
    public static IEnumerable<JpegSegment> ReadSegments(SequentialReader reader, ICollection<JpegSegmentType>? segmentTypes = null)
    {
        if (!reader.IsMotorolaByteOrder)
            throw new JpegProcessingException("Must be big-endian/Motorola byte order.");

        // first two bytes should be JPEG magic number
        var magicNumber = reader.GetUInt16();

        if (magicNumber != 0xFFD8)
            throw new JpegProcessingException($"JPEG data is expected to begin with 0xFFD8 (ÿØ) not 0x{magicNumber:X4}");

        do
        {
            // Find the segment marker. Markers are zero or more 0xFF bytes, followed
            // by a 0xFF and then a byte not equal to 0x00 or 0xFF.
            var segmentIdentifier = reader.GetByte();
            var segmentTypeByte = reader.GetByte();

            // Read until we have a 0xFF byte followed by a byte that is not 0xFF or 0x00
            while (segmentIdentifier != 0xFF || segmentTypeByte == 0xFF || segmentTypeByte == 0)
            {
                segmentIdentifier = segmentTypeByte;
                segmentTypeByte = reader.GetByte();
            }

            var segmentType = (JpegSegmentType)segmentTypeByte;

            if (segmentType == JpegSegmentType.Sos)
            {
                // The 'Start-Of-Scan' segment's length doesn't include the image data, instead would
                // have to search for the two bytes: 0xFF 0xD9 (EOI).
                // It comes last so simply return at this point
                yield break;
            }

            if (segmentType == JpegSegmentType.Eoi)
            {
                // the 'End-Of-Image' segment -- this should never be found in this fashion
                yield break;
            }

            // next 2-bytes are <segment-size>: [high-byte] [low-byte]
            var segmentLength = (int)reader.GetUInt16();

            // segment length includes size bytes, so subtract two
            segmentLength -= 2;

            // TODO exception strings should end with periods
            if (segmentLength < 0)
                throw new JpegProcessingException("JPEG segment size would be less than zero");

            // Check whether we are interested in this segment
            if (segmentTypes is null || segmentTypes.Contains(segmentType))
            {
                var segmentOffset = reader.Position;
                var segmentBytes = reader.GetBytes(segmentLength);
                Debug.Assert(segmentLength == segmentBytes.Length);
                yield return new JpegSegment(segmentType, segmentBytes, segmentOffset);
            }
            else
            {
                // Some of the JPEG is truncated, so just return what data we've already gathered
                if (!reader.TrySkip(segmentLength))
                    yield break;
            }
        }
        while (true);
    }
}
