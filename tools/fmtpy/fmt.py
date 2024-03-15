import argparse as _libargs
import os as _libos
import re as _libre
from multiprocessing.pool import Pool as ProcessPool
from pathlib import Path

import black as _libblack

# --------------------------------------------------------------------------------------------------

this_file = Path(_libos.path.realpath(__file__))
this_dir = this_file.parent.parent

# --------------------------------------------------------------------------------------------------

INCLUDE_PATH_PATTERNS = []

EXCLUDE_PATH_PATTERNS = [
    ".*__pycache__/.*",
    ".*node_modules/.*",
    ".*.git/.*",
    ".*.vs/.*",
    ".*build/.*",
    ".*dist/.*",
    ".*obj/.*",
    ".*bin/.*",
]

# --------------------------------------------------------------------------------------------------


def parse_arguments():
    argp = _libargs.ArgumentParser(
        prog=this_file.name,
        description="A Python formatter",
    )
    argp.add_argument(
        "--dir",
        dest="search_dir",
        type=Path,
        required=False,
        default=Path(_libos.getcwd()),
        help="A path to a directory to search",
    )
    argp.add_argument(
        "--include",
        dest="include_expr",
        type=str,
        required=False,
        default=None,
        help="A regular expression for paths to include",
    )
    argp.add_argument(
        "--exclude",
        dest="exclude_expr",
        type=str,
        required=False,
        default=None,
        help="A regular expression for paths to exclude",
    )
    argp.add_argument(
        "--verbose",
        dest="verbose",
        action="store_true",
        help="Enable verbose diagnostics",
    )
    return argp.parse_args()


# --------------------------------------------------------------------------------------------------


def format_py_file(file_path, args=None):
    try:
        posix_path = str(file_path.as_posix())
        if (
            (args.include_expr and _libre.match(args.include_expr, posix_path))
            or len(INCLUDE_PATH_PATTERNS) == 0
            or any([_libre.match(x, posix_path) for x in INCLUDE_PATH_PATTERNS])
        ) and not (
            (args.exclude_expr and _libre.match(args.exclude_expr, posix_path))
            or any([_libre.match(x, posix_path) for x in EXCLUDE_PATH_PATTERNS])
        ):
            if args.verbose:
                print(f"Formatting file: {posix_path}")
            code = file_path.read_text()
            if not code:
                print(f"Invalid input source for file: {posix_path}")
                return
            fcode = _libblack.format_str(code, mode=_libblack.FileMode())
            if not fcode:
                print(f"Invalid output source for file: {posix_path}")
                return
            file_path.write_text(fcode)
    except Exception as err:
        print(f"Caught exception for file: {file_path}\n{err}\n\n")


def format_py_dir(dir_path, args=None):
    path_generator = ((p, args) for p in dir_path.rglob("*.py") if p.is_file())
    ProcessPool().starmap(format_py_file, path_generator)


# --------------------------------------------------------------------------------------------------

if __name__ == "__main__":
    args = parse_arguments()
    if args.verbose:
        for k, v in args.__dict__.items():
            print(f"Using setting: {k} = {v}")
    format_py_dir(args.search_dir, args)
