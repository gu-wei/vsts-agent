# VSTS YAML definitions

This document focuses on the iterative plan to enable YAML definitions.

## Checkpoint 1 - Internal dogfooding

The main goal of this checkpoint is simply to enable YAML definitions for internal dogfooding. This goal does not require any UI changes.

A "YAML definifion" can be created by saving the definition with a variable `_yaml_preview` that specifies the path to the YAML file. When the definition is queued, relevant information will loaded from the YAML file instead of from the web definition. Add feature flag.

### Open questions

* Sign-off [deserialization features](yamldeserialization.md).
* Mustache context is ordinal-ignore-case
* Tasks
  - Error if task collision on name
  - Enforce `N.*` version. In the future, support finer granularity and even version omission with checked-in tasks.

## Checkpoint 2 - Public preview

Add UI to pick YAML file and disable/hide relevant portions of the web definition.

## Future checkpoint goals

* Resources
  - Endpoints
  - Specify queue in YAML
  - Sync multiple repos
  - Import/export verbs
* Process
  - Checked-in tasks
  - Template in separate repo (and policy)
  - Continue-on-error at job-level
  - Simplified syntax for command/script tasks
  - Use namespaced name for extensions tasks (clean way out of collision problem?)
* Server
  - Cache YAML files
  - Fallback to job/cancel timeout specified in web definition
* Definitions
  - Auto-create
  - Export to YAML
