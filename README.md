# SmartObjects

This project is a research to find ways to recycle instances of types that are used and disposed. The idea aims to avoid GarbageCollector activation since there will be some object reusage. 

* First idea 
 * Make a recyble bin that will serve as a repository of a given type. 
 * Provide a collection of items that will store cleared data to be stored in RecycleBin.
 * Provide a factory object to fetch a previously disposed item or create a new instance.
