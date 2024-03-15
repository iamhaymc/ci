# GitHub Actions

## Links

- [**Guides**](https://docs.github.com/en/actions)
    - https://docs.github.com/en/actions/using-workflows/workflow-syntax-for-github-actions
    - https://docs.github.com/en/actions/using-workflows/events-that-trigger-workflows
    - https://docs.github.com/en/actions/learn-github-actions/variables
    - https://docs.github.com/en/actions/security-guides/using-secrets-in-github-actions
    - https://docs.github.com/en/actions/using-workflows/reusing-workflows
- [**Marketplace**](https://github.com/marketplace?type=actions)
    - https://github.com/marketplace/actions/checkout
    - https://github.com/marketplace/actions/git-semantic-version
    - https://github.com/marketplace/actions/git-auto-commit
    - https://github.com/marketplace/actions/install-gcc
    - https://github.com/marketplace/actions/setup-python
    - https://github.com/marketplace/actions/setup-node-js-environment
    - https://github.com/marketplace/actions/setup-net-core-sdk
    - https://github.com/marketplace/actions/setup-msbuild
    - https://github.com/marketplace/actions/setup-msys2
    - https://github.com/marketplace/actions/build-and-push-docker-images
    - https://github.com/marketplace/actions/cache
    - https://github.com/marketplace/actions/upload-a-build-artifact
    - https://github.com/marketplace/actions/download-a-build-artifact
    - https://github.com/marketplace/actions/gh-release
    - https://github.com/marketplace/actions/github-pages-action
    - https://github.com/marketplace/actions/ssh-remote-commands
## Examples

```yaml
# When the "main" branch is pushed
#  - Check that branch out
#  - Install standard tooling
#  - Execute a CI script
name: push_main
on:
  push:
    branches:
      - main
    tags:
      - "v*.*.*"
jobs:
  run:
    runs-on: ubuntu-latest
    env:
      TARGET_ARCHITECTURE: x64
    steps:
      # ------------------------------------------------------------------------
      - name: Checkout Main
        uses: actions/checkout@v4
        with:
          fetch-depth: 0
          submodules: recursive

      # ------------------------------------------------------------------------
      - name: Install Python
        if: ${{ always() }}
        uses: actions/setup-python@v5
        with:
          python-version: "3.11"
          architecture: ${{ env.TARGET_ARCHITECTURE }}

      - name: Install Python packages
        if: ${{ always() }}
        shell: bash
        run: python -m pip install -r ci/pyreqs.txt

      # ------------------------------------------------------------------------
      - name: Install Node
        if: ${{ always() }}
        uses: actions/setup-node@v4
        with:
          node-version: "20.11.1"
          architecture: ${{ env.TARGET_ARCHITECTURE }}

      - name: Install Node packages
        if: ${{ always() }}
        shell: bash
        run: npm install

      # ------------------------------------------------------------------------
      - name: Install .NET
        if: ${{ always() }}
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: "8.x.x"

      # ------------------------------------------------------------------------
      - name: Install GCC
        if: ${{ always() }}
        uses: egor-tensin/setup-gcc@v1
        with:
          version: latest
          platform: ${{ env.TARGET_ARCHITECTURE }}

      # ------------------------------------------------------------------------
      - name: Execute Python Script
        if: ${{ always() }}
        shell: bash
        run: echo $(python ci/script.py) >> $GITHUB_STEP_SUMMARY
```