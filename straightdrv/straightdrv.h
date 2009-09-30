#ifndef __straightdrv_h__
#define __straightdrv_h__

#ifdef __GNUC__
#include <map>
using namespace std;
#define MAP_TYPE map
#else
#include <hash_map>
using namespace stdext;
#define MAP_TYPE hash_map
#endif

#endif
