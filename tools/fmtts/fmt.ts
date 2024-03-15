import * as Fs from "fs";
import * as Path from 'path';
import * as Url from 'url';
import { default as glob } from "fast-glob";
import * as prettier from "prettier";
import { format as formatCoffee } from "coffee-fmt";
import { SassFormatter } from "sass-formatter";
import { program as Cli } from 'commander';

// -------------------------------------------------------------------------------------------------

const SUPPORTED_EXTENSIONS = [
  'json', 'xml', 'yaml',
  'jo', 'mjo', "coffee",
  'js', 'mjs',
  'ts', 'mts',
  'sass', 'scss', 'css',
  'pug', 'html',
  'md'
];

const INCLUDE_PATH_PATTERNS = []

const EXCLUDE_PATH_PATTERNS = [
  ".*__pycache__/.*",
  ".*node_modules/.*",
  "package-lock.json$",
  ".*.git/.*",
  ".*.vs/.*",
  ".*build/.*",
  ".*dist/.*",
  ".*obj/.*",
  ".*bin/.*",
]

// -------------------------------------------------------------------------------------------------

// TODO: accept args, include/exclude paths

const _thisFile = Url.fileURLToPath(import.meta.url);
const _thisDir = Path.dirname(_thisFile);

(async () => {
  // Parse arguments
  const args = Cli
    .name(Path.basename(_thisFile))
    .description('A general media formatter')
    .option('--dir', 'A path to a directory to search', process.cwd())
    .option('--include', 'A regular expression for paths to include', <any>null)
    .option('--exclude', 'A regular expression for paths to exclude', <any>null)
    .option('--verbose', 'Enable verbose diagnostics', <any>true)
    .parse(process.argv)?.opts() ?? {};
  if (args.verbose)
    for (let k in args)
      console.log(`Using setting: ${k} = ${args[k]}`)
  // Search files
  let pathPattern = `**/*.{${SUPPORTED_EXTENSIONS.join(',')}}`;
  for (let path of await glob([pathPattern], { dot: true, base: args.dir })) {
    try {
      // Interrogate file
      if (
        (args.include && path.match(args.include)
          || INCLUDE_PATH_PATTERNS.length == 0
          || INCLUDE_PATH_PATTERNS.some(x => path.match(x)))
        && !((args.exclude && path.match(args.exclude))
          || EXCLUDE_PATH_PATTERNS.some(x => path.match(x)))
      ) {
        const prettierOptions = {
          parser: <any>null,
          plugins: ["@prettier/plugin-xml", "@prettier/plugin-pug"],
        };
        const info = await prettier.getFileInfo(path, {
          ignorePath: ".prettierignore",
          plugins: prettierOptions.plugins,
        });

        if (!info.ignored) {
          // Determine parser
          let extension = Path.extname(path)?.toLowerCase();
          switch (extension) {
            case ".json":
              prettierOptions.parser = "json";
              break;
            case ".xml":
              prettierOptions.parser = "xml";
              break;
            case ".yaml":
              prettierOptions.parser = "yaml";
              break;
            case ".jo":
            case ".mjo":
            case ".coffee":
              prettierOptions.parser = "coffee";
              break;
            case ".js":
            case ".mjs":
              prettierOptions.parser = "babel";
              break;
            case ".ts":
            case ".mts":
              prettierOptions.parser = "typescript";
              break;
            case ".css":
              prettierOptions.parser = "css";
              break;
            case ".scss":
              prettierOptions.parser = "scss";
              break;
            case ".sass":
              prettierOptions.parser = "sass";
              break;
            case ".pug":
              prettierOptions.parser = "pug";
              break;
            case ".html":
              prettierOptions.parser = "html";
              break;
            case ".md":
              prettierOptions.parser = "markdown";
              break;
            default:
              prettierOptions.parser =
                prettierOptions.parser ?? info.inferredParser;
              break;
          }

          if (prettierOptions.parser) {
            if (prettierOptions.parser === "coffee") {
              // Format with CoffeeScript formatter
              // ---------------------------------------------------------------
              if (args.verbose)
                console.log(`Formatting file: ${path}`);
              let code = Fs.readFileSync(path, "utf-8");
              if (!code) {
                console.error("Invalid input source for file: " + path);
                continue;
              }
              let fcode = formatCoffee(code, {
                tab: "  ",
                newLine: "\n",
                debug: false,
              })?.toString();
              if (!fcode) {
                console.error("Invalid output source for file: " + path);
                continue;
              }
              Fs.writeFileSync(path, fcode, "utf-8");
              
            } else if (prettierOptions.parser === "sass") {
              // Format with Sass formatter
              // ---------------------------------------------------------------
              if (args.verbose)
                console.log(`Formatting file: ${path}`);
              let code = Fs.readFileSync(path, "utf-8");
              if (!code) {
                console.error("Invalid input source for file: " + path);
                continue;
              }
              let fcode = SassFormatter.Format(code, {
                tabSize: 2,
                insertSpaces: true,
                lineEnding: "LF",
                deleteEmptyRows: true,
                setPropertySpace: true,
                debug: false,
              });
              if (!fcode) {
                console.error("Invalid output source for file: " + path);
                continue;
              }
              Fs.writeFileSync(path, fcode, "utf-8");

            } else {
              // Format with Prettier formatter
              // ---------------------------------------------------------------
              let code = Fs.readFileSync(path, "utf-8");
              if (!code) {
                console.error("Invalid input source for file: " + path);
                continue;
              }
              if (!(await prettier.check(code, prettierOptions))) {
                if (args.verbose)
                  console.log(`Formatting file: ${path}`);
                let fcode = await prettier.format(code, prettierOptions);
                if (!fcode) {
                  console.error("Invalid output source for file: " + path);
                  continue;
                }
                Fs.writeFileSync(path, fcode, "utf-8");
              }
            }
          }
        }
      }
    } catch (err) {
      console.error(`Caught exception for file: ${path}\n${err}\n\n`);
    }
  }
})();