use std::fs;

use crate::version::Version;

pub fn update_csharp(version: &Version) {
	update_csharp_file(&version, r"..\site\AssetPackages\AssetPackages.csproj");
	update_csharp_file(&version, r"..\site\Authentication\Authentication.csproj");
	update_csharp_file(&version, r"..\site\BusinessLogic\BusinessLogic.csproj");
	update_csharp_file(&version, r"..\site\Email\Email.csproj");
	update_csharp_file(&version, r"..\site\Entities\Entities.csproj");
	update_csharp_file(&version, r"..\site\Generic\Generic.csproj");
	update_csharp_file(&version, r"..\site\Language\Language.csproj");
	update_csharp_file(&version, r"..\site\MVC\MVC.csproj");
	update_csharp_file(&version, r"..\site\Tests\BusinessLogic\BusinessLogic.Tests.csproj");
	update_csharp_file(&version, r"..\site\Tests\Email\Email.Tests.csproj");
	update_csharp_file(&version, r"..\site\Tests\Language\Language.Tests.csproj");
	update_csharp_file(&version, r"..\site\Tests\MVC\MVC.Tests.csproj");
	update_csharp_file(&version, r"..\site\Tests\Util\Tests.Util.csproj");
}

fn update_csharp_file(version: &Version, file: &str) {
	let old_assembly = assembly_version(&version.prev);
	let new_assembly = assembly_version(&version.code);

	let old_file = assembly_file_version(&version.prev);
	let new_file = assembly_file_version(&version.code);
	
	let content = fs::read_to_string(file)
		.unwrap()
		.replace(&old_assembly, &new_assembly)
		.replace(&old_file, &new_file);

	fs::write(file, content)
		.expect(&format!("error on c# {} recording", file));
}

fn assembly_version(version: &str) -> String {
	format!(r#"    <AssemblyVersion>{}</AssemblyVersion>"#, version)
}

fn assembly_file_version(version: &str) -> String {
	format!(r#"<FileVersion>{}</FileVersion>"#, version)
}
