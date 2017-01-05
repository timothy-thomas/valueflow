grammar MuParser;

/*
 * Parser Rules
 */

test
 : expr ('\n' expr)*  # testExpr
 ;

expr
 : expr POW<assoc=right> expr                   # powExpr
 | SUB expr                                     # unaryMinusExpr
 | expr op=(MUL | DIV) expr                     # mulDivExpr
 | expr op=(ADD | SUB) expr                     # addSubExpr
 | expr op=(LTEQ | GTEQ | LT | GT) expr         # relationalExpr
 | expr op=(EQ | NEQ) expr                      # equalityExpr
 | expr AND expr                                # andExpr
 | expr OR expr                                 # orExpr
 | expr QUESTION expr COLON expr                # iteExpr
 | op=FUNCTION OPAR expr CPAR                   # functionExpr
 | op=FUNCTIONMULTI OPAR expr (',' expr)* CPAR  # functionMultiExpr
 | atom                                         # atomExpr
 | op=( ASSIGN
      | ASSIGNADD
      | ASSIGNSUB
      | ASSIGNMUL
      | ASSIGNDIV)                              # assignExpr
 ;

atom
 : OPAR expr CPAR     # parExpr
 | (INT | FLOAT)      # numberAtom
 | (TRUE | FALSE)     # booleanAtom
 | (E | PI)           # predefinedConstantAtom
 | ID                 # idAtom
 ;

compileUnit
 :	EOF
 ;

/*
 * Lexer Rules
 */

FUNCTION
 : 'sin'
 | 'cos'
 | 'tan'
 | 'asin'
 | 'acos'
 | 'atan'
 | 'sinh'
 | 'cosh'
 | 'tanh'
 | 'asinh'
 | 'acosh'
 | 'atanh'
 | 'log2'
 | 'log10'
 | 'log'
 | 'ln'
 | 'exp'
 | 'sqrt'
 | 'sign'
 | 'rint'
 | 'abs'
 ;

FUNCTIONMULTI
 : 'min'
 | 'max'
 | 'sum'
 | 'avg'
 ;

ASSIGN    : '=' ;
ASSIGNADD : '+=' ;
ASSIGNSUB : '-=' ;
ASSIGNMUL : '*=' ;
ASSIGNDIV : '/=' ;
AND       : '&&' ;
OR        : '||' ;
LTEQ      : '<=' ;
GTEQ      : '>=' ;
NEQ       : '!=' ;
EQ        : '==' ;
LT        : '<' ;
GT        : '>' ;
ADD       : '+' ;
SUB       : '-' ;
MUL       : '*' ;
DIV       : '/' ;
POW       : '^' ;
NOT       : '!' ;

QUESTION  : '?' ;
COLON     : ':' ;

OPAR
 : '('
 ;

CPAR
 : ')'
 ;

INT
 : [0-9]+
 ;

FLOAT
 : [0-9]+ '.' [0-9]* 
 | '.' [0-9]+
 ;

TRUE
 : 'true'
 ;

FALSE
 : 'false'
 ;

E
 : '_e'
 ;

PI
 : '_pi'
 ;

ID
 : [a-zA-Z_] [a-zA-Z_0-9]*
 ;

SPACE
 : [ \t\r\n] -> skip
 ;