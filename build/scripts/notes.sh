#!/usr/bin/env bash

# creates a skeleton for release notes

cat > notes <<EOF
_funne subtitle_

general tl;dr

Tested with Muse Dash VERSION aka UPDATE NAME, released $(date -I).

## Changelog
tl;dr mentioning changed mods

### Song info
Now VERSION, optional tl;dr:
- Support for VERSION's new OPTIONAL DESC songs out of the box

---
The rest of the mods are re-released.

## Checksums
Don't forget to verify your downloads! SHA256 checksums for DLLs:
\`\`\`
$(cat checksums)
\`\`\`
EOF
