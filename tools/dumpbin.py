import argparse as _libargs
import os as _libos
import re as _libre
import shutil as _libsh
import subprocess as _libproc
from itertools import dropwhile, takewhile
from pathlib import Path
from types import SimpleNamespace

import colorama
from colorama import Fore as clifg
from colorama import Style as clisty

# --------------------------------------------------------------------------------------------------

this_file = Path(_libos.path.realpath(__file__))
this_dir = this_file.parent.parent
colorama.init()


# ------------------------------------------------------------------------------------------------


def _get_dumpbin_path():
    dumpbin_exe = _libsh.which("dumpbin")
    if not dumpbin_exe:
        raise Exception('"dumpbin" was not found on the environment PATH')
    return dumpbin_exe


# ------------------------------------------------------------------------------------------------


def _dump_bin_type(bin_path):

    dumpbin_exe = _get_dumpbin_path()
    res = _libproc.run([dumpbin_exe, "/headers", str(bin_path)], capture_output=True)
    text = res.stdout.decode("utf-8").strip() + "\n"
    matches = _libre.findall("\s*File Type: ([^\r\n]+)", text)
    type = f"{bin_path.suffix[1:]}?" if len(matches) == 0 else matches[0]
    matches = _libre.findall("\s*machine ([^\r\n]+)", text)
    type += f' {"" if len(matches) == 0 else matches[0]}'
    type = type.upper()

    return type


# --------------------------------------------------------------------------------------------------


def _dump_bin_depends(bin_path, dep_map={}, depth=0):

    dumpbin_exe = _get_dumpbin_path()
    res = _libproc.run([dumpbin_exe, "/dependents", str(bin_path)], capture_output=True)
    text = res.stdout.decode("utf-8").strip() + "\n"
    lines = text.splitlines(keepends=False)
    lines = dropwhile(lambda x: not "has the following dependencies" in x, lines)
    deps = [x.strip() for x in takewhile(lambda x: x != "", list(lines)[2:])]

    for d in deps:
        key = d.lower()
        if key not in dep_map:
            dep_info = SimpleNamespace()
            dep_info.name = key
            dep_info.path = bin_path.with_name(dep_info.name)
            dep_info.type = _dump_bin_type(dep_info.path)
            dep_map[key] = dep_info
            _dump_bin_depends(dep_info.path, dep_map, depth + 1)
        dep_map[key].depth = depth

    return dep_map


# --------------------------------------------------------------------------------------------------


def _dump_bin_exports(bin_path, dep_map={}, depth=0):

    dumpbin_exe = _get_dumpbin_path()
    res = _libproc.run([dumpbin_exe, "/exports", str(bin_path)], capture_output=True)
    text = res.stdout.decode("utf-8").strip() + "\n"
    lines = text.splitlines(keepends=False)
    lines = list(dropwhile(lambda x: not "ordinal hint RVA" in x, lines))[2:]
    exports = [x.split(None)[-1] for x in takewhile(lambda x: x != "", lines)]

    return exports


# --------------------------------------------------------------------------------------------------


def dump_bin(bin_path):

    info = SimpleNamespace()
    info.path = Path(bin_path)
    info.name = info.path.name.lower()
    info.type = _dump_bin_type(info.path)
    info.depends = _dump_bin_depends(info.path).values()
    info.exports = _dump_bin_exports(info.path)

    return info


# --------------------------------------------------------------------------------------------------


def print_bin(info, display_depends=True, display_exports=False):

    summary = f"[BINARY]\n"
    summary += f" >  {clifg.WHITE + clisty.BRIGHT + info.name + clisty.RESET_ALL}\n"
    summary += f"    {clifg.BLACK}{info.type}{clisty.RESET_ALL}\n"

    if display_depends:
        summary += f"\n[DEPENDS]\n"
        for x in sorted(info.depends, key=lambda x: str(x.depth) + x.name):
            disp_symbol = (
                f"{clifg.GREEN}v{clisty.RESET_ALL}"
                if x.path.exists
                else f"{clifg.RED}x{clisty.RESET_ALL}"
            )
            disp_name = f"{clifg.WHITE}{clisty.DIM if x.depth > 1 else clisty.BRIGHT}{x.name}{clisty.RESET_ALL}"
            disp_info = (
                f"{clifg.BLACK}*{x.depth}  {clifg.BLACK}{x.type}{clisty.RESET_ALL}"
            )
            summary += f" {disp_symbol}  {disp_info:30}  {disp_name} \n"

    if display_exports:
        summary += f"\n[EXPORTS]\n"
        for x in sorted(info.exports):
            summary += f' {clifg.BLUE + "$" + clisty.RESET_ALL}  {x}\n'

    print(summary)


# ==================================================================================================

if __name__ == "__main__":
    argp = _libargs.ArgumentParser(
        prog=this_file.name,
        description="A binary interrogator",
    )
    argp.add_argument(
        "--bin",
        dest="bin_path",
        type=Path,
        required=True,
        help="A path to a binary",
    )
    args = argp.parse_args()

    info = dump_bin(args.bin_path)
    print_bin(info)
