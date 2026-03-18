# UV package

To use the python file and all its imports, you need to install UV - 

Install uv with our standalone installers:

# On macOS and Linux.
curl -LsSf https://astral.sh/uv/install.sh | sh

# On Windows.
powershell -ExecutionPolicy ByPass -c "irm https://astral.sh/uv/install.ps1 | iex"

after that, run uv sync to install all the dependencies specified in the uv.lock file. 

You should be able to run it now, but you may also enter the environment in your ide to get rid of all the squiggly lines c: