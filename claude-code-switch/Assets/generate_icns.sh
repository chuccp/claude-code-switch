#!/bin/bash
# Generate icns from SVG icon
# Requires inkscape

# Create iconset directory
mkdir -p claude-code-switch.iconset

# Generate different sizes
sizes=(16 32 64 128 256 512 1024)

for size in "${sizes[@]}"; do
    echo "Generating ${size}x${size}..."
    inkscape -w $size -h $size icon.svg -o claude-code-switch.iconset/icon_${size}x${size}.png
done

# Generate @2x versions
for size in 16 32 64 128 256 512; do
    double=$((size * 2))
    echo "Generating ${double}x${double} @2x..."
    inkscape -w $double -h $double icon.svg -o claude-code-switch.iconset/icon_${size}x${size}@2x.png
done

# Convert to icns
echo "Converting to claude-code-switch.icns..."
iconutil -c icns claude-code-switch.iconset

# Clean up
rm -rf claude-code-switch.iconset

echo "Done! Output: claude-code-switch.icns"
echo "Move this to claude-code-switch.app/Contents/Resources/AppIcon.icns"
