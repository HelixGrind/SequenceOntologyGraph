# SequenceOntologyGraph

## Prerequisites

* Download the [so-simple.obo](https://github.com/The-Sequence-Ontology/SO-Ontologies/raw/master/Ontology_Files/so-simple.obo) file from the [SO-Ontologies repo](https://github.com/The-Sequence-Ontology/SO-Ontologies).
* Install [Graphviz](https://graphviz.org/download/)

## Output

![Pruned SO graph starting at transcript_variant](https://github.com/MichaelStromberg-Illumina/SequenceOntologyGraph/raw/main/Data/sequence_ontology_simple_transcript.svg)

## Usage

```
dotnet Obo.dll so-simple.obo sequence_ontology_simple_transcript.dot transcript_variant Y
dot -Tsvg sequence_ontology_simple_transcript.dot -o sequence_ontology_simple_transcript.svg
```
