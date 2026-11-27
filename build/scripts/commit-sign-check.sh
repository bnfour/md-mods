#!/usr/bin/env bash

# A script that verifies that the HEAD is signed, because I can forget to do that.

# creates an allowed-signers file that assumes that HEAD was done by repo owner
# (works for me on small me-only repos, ymmv)

# commit author email
email=$(git show --no-patch --format=%ae HEAD)

# configured _signing_ SSH keys for the repo owner account
# gh is available on runners and is shorter that curling with all the headers
# (as long as you don't forget to set GH_TOKEN in env)
keys=$(gh api "users/{owner}/ssh_signing_keys" \
    | jq -r '.[].key')

if [[ -z "${keys[*]}" ]]
then
    echo "No SSH signing keys found for repo owner."
    exit 1
fi

file="$RUNNER_TEMP/allowed-signers"
for key in "${keys[@]}";
do
    echo "$email $key" >> "$file"
done

git config --local gpg.format ssh
git config --local gpg.ssh.allowedsignersfile "$file"

if git verify-commit HEAD &>/dev/null
then
    echo "d_(^v^)_b"
else
    echo "he forgor"
    exit 1
fi
