String Stack
============

Ulrich Hoffmann <uho@xlerb.de>

Version 1.0.2 - 2015-10-03

This package provides an implementation of a string stack to Forth-94 and Forth-2012 systems.
It is based on a string stack implementation by Klaus Schliesiek from 1986 but instead of
character counted strings it uses cell counts. No interpretation of the strings takes place so
it can as well process arbitrary binary data.

## Installation

To use the string stack, just `INCLUDE stringstack.fs` on any standard system. This makes
the string stack definitions available. After loading you still have a standard system.

## Documentation

See the file [**glossary.md**](glossary.md) for a description of the defined words.

## Example usage

Here are some examples how to use the string stack:

Put a string on the string stack and print it

`" a string" ".`  **a string ok**

Do string stack manipulations

`" a string"  "dup   ". ".`  **a string  a string ok**

Join strings

`" Forth String World!"   " Hello, "   "join  ".`  **Hello, Forth String World! ok**

Split a string at a given delimiter, the join the resulting strings with another delimiter

`" sing,sang,song" " ,"  "delimiter-split   "  - " "delimiter-join ".`  **sing - sang - song ok**

Substitute a string within a text by some other string

`" name=$(name)"  " $(name)"  " Forth"  "substitute ".`   **name=Forth ok**

## Bug Reports

Please send bug reports, umprovements and suggestions to Ulrich Hoffmann <<uho@xlerb.de>>

## Conformance

This program conforms to Forth-94 and Forth-2012

May the Forth be with you!
