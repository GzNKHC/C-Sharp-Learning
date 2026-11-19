<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="Current" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"
          Condition="Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')" />
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">x64</Platform>
    <ProjectGuid>{1B565AA4-D7AA-44BB-AEF4-060F30121864}</ProjectGuid>
    <OutputType>WinExe</OutputType>
    <RootNamespace>ThucHanh8</RootNamespace>
    <AssemblyName>TH08</AssemblyName>
    <PlatformTarget>x64</PlatformTarget>
    <TargetFrameworkVersion>v4.8</TargetFrameworkVersion>
    <LangVersion>7.3</LangVersion>
    <OutputPath>bin\$(Configuration)\</OutputPath>
    <AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)' == 'Debug' ">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)' == 'Release' ">
    <Optimize>true</Optimize>
    <DefineConstants>TRACE</DefineConstants>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="System" />
    <Reference Include="System.Configuration" />
    <Reference Include="System.Core" />
    <Reference Include="System.Data" />
    <Reference Include="System.Drawing" />
    <Reference Include="System.Windows.Forms" />
    <Reference Include="System.Xml" />
  </ItemGroup>
  <ItemGroup>
    <Reference Include="CrystalDecisions.CrystalReports.Engine">
      <HintPath Condition="'$(CrystalAssembliesPath)' != ''">$(CrystalAssembliesPath)\CrystalDecisions.CrystalReports.Engine.dll</HintPath>
      <Private>false</Private>
    </Reference>
    <Reference Include="CrystalDecisions.ReportAppServer.ClientDoc">
      <HintPath Condition="'$(CrystalAssembliesPath)' != ''">$(CrystalAssembliesPath)\CrystalDecisions.ReportAppServer.ClientDoc.dll</HintPath>
      <Private>false</Private>
    </Reference>
    <Reference Include="CrystalDecisions.ReportAppServer.CommonObjectModel">
      <HintPath Condition="'$(CrystalAssembliesPath)' != ''">$(CrystalAssembliesPath)\CrystalDecisions.ReportAppServer.CommonObjectModel.dll</HintPath>
      <Private>false</Private>
    </Reference>
    <Reference Include="CrystalDecisions.ReportAppServer.CommLayer">
      <HintPath Condition="'$(CrystalAssembliesPath)' != ''">$(CrystalAssembliesPath)\CrystalDecisions.ReportAppServer.CommLayer.dll</HintPath>
      <Private>false</Private>
    </Reference>
    <Reference Include="CrystalDecisions.ReportAppServer.Controllers">
      <HintPath Condition="'$(CrystalAssembliesPath)' != ''">$(CrystalAssembliesPath)\CrystalDecisions.ReportAppServer.Controllers.dll</HintPath>
      <Private>false</Private>
    </Reference>
    <Reference Include="CrystalDecisions.ReportAppServer.DataDefModel">
      <HintPath Condition="'$(CrystalAssembliesPath)' != ''">$(CrystalAssembliesPath)\CrystalDecisions.ReportAppServer.DataDefModel.dll</HintPath>
      <Private>false</Private>
    </Reference>
    <Reference Include="CrystalDecisions.ReportAppServer.DataSetConversion">
      <HintPath Condition="'$(CrystalAssembliesPath)' != ''">$(CrystalAssembliesPath)\CrystalDecisions.ReportAppServer.DataSetConversion.dll</HintPath>
      <Private>false</Private>
    </Reference>
    <Reference Include="CrystalDecisions.ReportAppServer.ReportDefModel">
      <HintPath Condition="'$(CrystalAssembliesPath)' != ''">$(CrystalAssembliesPath)\CrystalDecisions.ReportAppServer.ReportDefModel.dll</HintPath>
      <Private>false</Private>
    </Reference>
    <Reference Include="CrystalDecisions.Shared">
      <HintPath Condition="'$(CrystalAssembliesPath)' != ''">$(CrystalAssembliesPath)\CrystalDecisions.Shared.dll</HintPath>
      <Private>false</Private>
    </Reference>
    <Reference Include="CrystalDecisions.Windows.Forms">
      <HintPath Condition="'$(CrystalAssembliesPath)' != ''">$(CrystalAssembliesPath)\CrystalDecisions.Windows.Forms.dll</HintPath>
      <Private>false</Private>
    </Reference>
  </ItemGroup>
  <ItemGroup>
    <Compile Include="Db.cs" />
    <Compile Include="EditForm.cs" />
    <Compile Include="PrintGrid.cs" />
    <Compile Include="Program.cs" />
    <Compile Include="ReceiptForm.cs" />
    <Compile Include="ReceiptStore.cs" />
    <Compile Include="StudentForm.cs" />
    <Compile Include="StudentReportData.cs" />
    <Compile Include="StudentReportForm.cs" />
    <Compile Include="StudentReportTemplate.cs" />
    <Compile Include="Ui.cs" />
    <None Include="App.config" />
    <None Include="Setup.sql" />
    <None Include="Reports\BaseReport-LICENSE.txt" />
    <Content Include="Reports\BaseReport.rpt">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
    <Content Include="Reports\MyReport.rpt" Condition="Exists('Reports\MyReport.rpt')">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
  </ItemGroup>
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
</Project>
