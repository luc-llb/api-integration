#!/bin/bash

# Script to install sqlcmd in Ubuntu

echo "Installing sqlcmd in Codespace..."

# 1. Check Ubuntu version
echo "Detecting Ubuntu version..."
UBUNTU_VERSION=$(lsb_release -rs)
echo "Ubuntu version: $UBUNTU_VERSION"

# 2. Update system
sudo apt-get update

# 3. Install dependencies
sudo apt-get install -y curl apt-transport-https gnupg lsb-release

# 4. Remove old Microsoft repositories (if they exist)
sudo rm -f /etc/apt/sources.list.d/mssql-release.list
sudo rm -f /etc/apt/sources.list.d/prod.list

# 5. Add Microsoft GPG key (updated method)
curl -fsSL https://packages.microsoft.com/keys/microsoft.asc | sudo gpg --dearmor -o /usr/share/keyrings/microsoft-prod.gpg

# 6. Add repository based on Ubuntu version
if [[ "$UBUNTU_VERSION" == "22.04" ]]; then
    echo "deb [arch=amd64,arm64,armhf signed-by=/usr/share/keyrings/microsoft-prod.gpg] https://packages.microsoft.com/ubuntu/22.04/prod jammy main" | sudo tee /etc/apt/sources.list.d/mssql-release.list
elif [[ "$UBUNTU_VERSION" == "20.04" ]]; then
    echo "deb [arch=amd64,arm64,armhf signed-by=/usr/share/keyrings/microsoft-prod.gpg] https://packages.microsoft.com/ubuntu/20.04/prod focal main" | sudo tee /etc/apt/sources.list.d/mssql-release.list
elif [[ "$UBUNTU_VERSION" == "18.04" ]]; then
    echo "deb [arch=amd64 signed-by=/usr/share/keyrings/microsoft-prod.gpg] https://packages.microsoft.com/ubuntu/18.04/prod bionic main" | sudo tee /etc/apt/sources.list.d/mssql-release.list
else
    # Fallback to Ubuntu 20.04 if detection fails
    echo "Using default configuration for Ubuntu 20.04..."
    echo "deb [arch=amd64,arm64,armhf signed-by=/usr/share/keyrings/microsoft-prod.gpg] https://packages.microsoft.com/ubuntu/20.04/prod focal main" | sudo tee /etc/apt/sources.list.d/mssql-release.list
fi

# 7. Update package list
sudo apt-get update

# 8. Install mssql-tools
echo "Installing mssql-tools..."
sudo ACCEPT_EULA=Y apt-get install -y mssql-tools unixodbc-dev

# 9. Add to PATH
echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc

# 10. Apply changes
export PATH="$PATH:/opt/mssql-tools/bin"

echo ""
echo "✅ Installation complete!"
echo "Run 'source ~/.bashrc' or open a new terminal to use sqlcmd"
echo "Test with: sqlcmd -?"