SOURCES := $(shell find src/ -name '*.cs')

sharpie.exe: $(SOURCES)
	dmcs -out:$@ $^
